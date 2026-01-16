using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.Commands;

namespace TaskApplication.Tasks.Handlers
{
    /// <summary>
    /// Handler to update all tasks
    /// </summary>
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskDbContext _context;
        private readonly HybridCache _hybridCache;


        public UpdateTaskCommandHandler(ITaskDbContext context, HybridCache hybridCache)
        {
            _context = context;
            _hybridCache = hybridCache;

        }
        /// <summary>
        /// Updates a record in the database if found
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.StatusID = request.StatusId;

            await _context.SaveChangesAsync(cancellationToken);

            await _hybridCache.RemoveAsync($"task-{request.Id}", cancellationToken);

            // Remove the "All Tasks" list cache because the data in that list is now stale
            await _hybridCache.RemoveAsync("tasks-all", cancellationToken);


            return true;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.Commands;

namespace TaskApplication.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskDbContext _context;

        public UpdateTaskCommandHandler(ITaskDbContext context)
        {
            _context = context;
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

            return true;
        }
    }
}

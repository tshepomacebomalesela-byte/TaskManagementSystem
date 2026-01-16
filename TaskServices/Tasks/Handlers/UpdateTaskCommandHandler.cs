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

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. Retrieve the existing entity
            var entity = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            // 2. Handle "Not Found" case
            if (entity == null)
            {
                return false;
                // In a more advanced setup, you might throw a custom 'NotFoundException'
            }

            // 3. Update the properties
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.StatusID = request.StatusId;

            // 4. Save changes
            // EF Core tracks the changes on 'entity' automatically
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

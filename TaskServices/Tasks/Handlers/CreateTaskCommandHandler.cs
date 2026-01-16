using MediatR;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.Commands;

namespace TaskApplication.Tasks.Handlers
{
    public class CreateTasksCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly ITaskDbContext _context;
        public CreateTasksCommandHandler(ITaskDbContext context) => _context = context;

        /// <summary>
        /// Method to create new tasks and store them in the postgres db
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int> Handle(CreateTaskCommand request, CancellationToken ct)
        {
            if((request.StatusId<0 || request.StatusId>4) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Description))
            {
                throw new InvalidOperationException("Could not add Task");
            }

            var entity = new TaskDomain.Task
            {
                Name = request.Name,
                Description = request.Description,
                StatusID = request.StatusId
            };

            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity.Id;

        }
    }
}

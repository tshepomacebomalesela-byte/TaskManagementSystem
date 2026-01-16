using MediatR;
using TaskApplication.Tasks.DTOs;

namespace TaskApplication.Tasks.Queries
{
    public record GetAllTasksQuery : IRequest<List<TaskDTO>>;
}

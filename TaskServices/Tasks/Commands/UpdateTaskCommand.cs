using MediatR;
namespace TaskApplication.Tasks.Commands
{
    public record UpdateTaskCommand(int Id, string Name, string Description, int StatusId) : IRequest<bool>;
}

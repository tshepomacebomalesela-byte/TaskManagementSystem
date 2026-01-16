using MediatR;
namespace TaskApplication.Tasks.Commands
{
    public record CreateTaskCommand(string Name, string Description, int StatusId) : IRequest<int>;
}

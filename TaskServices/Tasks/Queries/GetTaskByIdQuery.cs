using MediatR;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.DTOs;

namespace TaskApplication.Tasks.Queries
{
    public record GetTaskByIdQuery(int Id) : IRequest<TaskDTO?>, ICachableQuery
    {
        public string CacheKey => $"tasks-{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    };
}

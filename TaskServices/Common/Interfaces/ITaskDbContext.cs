using Microsoft.EntityFrameworkCore;

namespace TaskApplication.Common.Interfaces
{
    public interface ITaskDbContext
    {
        DbSet<TaskDomain.Task> Tasks { get; }
        DbSet<TaskDomain.Status> Status { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

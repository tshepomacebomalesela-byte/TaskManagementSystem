using Microsoft.EntityFrameworkCore;
using TaskApplication.Common.Interfaces;
using TaskDomain;

namespace TaskInfrastructure.Persistance
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskDomain.Task> Tasks => Set<TaskDomain.Task>();
        public DbSet<Status> Status => Set<Status>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

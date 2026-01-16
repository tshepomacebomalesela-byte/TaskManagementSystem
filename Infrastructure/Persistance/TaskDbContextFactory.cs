using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace TaskInfrastructure.Persistance
{
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TaskManagementDB;Username=tshepo@1234;Password=Tshepo@123");

            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}

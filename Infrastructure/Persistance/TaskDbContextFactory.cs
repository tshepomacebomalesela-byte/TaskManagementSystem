using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskInfrastructure.Persistance
{
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
            // Hardcode your dev connection string here just for migrations
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TaskManagementDB;Username=tshepo@1234;Password=Tshepo@123");

            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}

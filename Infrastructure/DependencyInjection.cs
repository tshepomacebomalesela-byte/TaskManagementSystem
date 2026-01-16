using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskApplication.Common.Interfaces;
using TaskInfrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace TaskInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register the DbContext with your PostgreSQL connection string
            services.AddDbContext<TaskDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TaskDbContext).Assembly.FullName)));

            // 2. IMPORTANT: Map the interface to the concrete DbContext class
            // This resolves the error you are receiving.
            services.AddScoped<ITaskDbContext>(provider => provider.GetRequiredService<TaskDbContext>());

            return services;
        }
    }
}

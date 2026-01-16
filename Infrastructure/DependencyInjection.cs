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
            services.AddDbContext<TaskDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TaskDbContext).Assembly.FullName)));

            services.AddScoped<ITaskDbContext>(provider => provider.GetRequiredService<TaskDbContext>());

            return services;
        }
    }
}

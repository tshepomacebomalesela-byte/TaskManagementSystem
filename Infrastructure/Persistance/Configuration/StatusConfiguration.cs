using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskDomain;

namespace TaskInfrastructure.Persistance.Configuration
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasData(
                new Status { Id = 1, Name = "Open" },
                new Status { Id = 2, Name = "Closed" },
                new Status { Id = 3, Name = "Pending" },
                new Status { Id = 4, Name = "On-Hold" }
            );
        }
    }
}

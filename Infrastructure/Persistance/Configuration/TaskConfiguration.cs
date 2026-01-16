using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskInfrastructure.Persistance.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskDomain.Task>
    {
        public void Configure(EntityTypeBuilder<TaskDomain.Task> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Status)
                   .WithMany()
                   .HasForeignKey(i => i.StatusID)
                   .OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        }
    }
}

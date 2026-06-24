using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Data.Configuration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {

            builder.Property(p => p.Name)
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationDays",
                    "DurationDays BETWEEN 1 AND 365");
            });

            builder.HasIndex(b => b.Name)
                .IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}

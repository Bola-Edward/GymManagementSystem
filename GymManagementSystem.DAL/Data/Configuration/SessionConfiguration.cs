using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configuration
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Session_Capacity", "[Capacity] BETWEEN 1 AND 25");
                t.HasCheckConstraint("CK_Session_DateRange", "[StartDate] < [EndDate]");
            });

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}

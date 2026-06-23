using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Configuration
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(m => m.StartDate)
                .IsRequired();

            builder.Property(m => m.EndDate)
                .IsRequired();

            builder.HasOne(m => m.Member)
                .WithMany(m => m.Memberships)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Plan)
                .WithMany(p => p.MemberShips)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_MemberShip_DateRange", "[StartDate] < [EndDate]");
            });



            builder.HasQueryFilter(ms => !ms.IsDeleted);
        }
    }
}

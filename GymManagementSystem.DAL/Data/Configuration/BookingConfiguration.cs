using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.Property(x => x.IsAttended)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Session)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => new
            {
                x.MemberId,
                x.SessionId
            }).IsUnique();


            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Booking_Date", "[Date] >= GETDATE()");
            });

            builder.HasQueryFilter(B => !B.IsDeleted);
        }
    }
}

using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Configuration
{
    public class MemberConfiguration : UserConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);

            // configuration related to the Memeber entity can be added here if needed
            builder.Property(p => p.Photo)
                .HasMaxLength(1000);

        }
    }
}

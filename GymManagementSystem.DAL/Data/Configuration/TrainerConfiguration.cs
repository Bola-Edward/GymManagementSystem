using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Configuration
{
    public class TrainerConfiguration : UserConfiguration<Trainer>
    {
        public override void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);

            // configuration related to the Trainer entity can be added here if needed

            builder.Property(p => p.Speciality)
                .HasConversion<string>()
                .HasMaxLength(30);

        }
    }
}

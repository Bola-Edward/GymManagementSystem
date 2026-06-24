using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagementSystem.DataAccess.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(u => u.Name)
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            builder.OwnsOne(x => x.Address, t =>
            {
                t.Property(a => a.Street)
                .HasColumnName("Street")
                .HasMaxLength(100);

                t.Property(a => a.City)
                .HasColumnName("City")
                .HasMaxLength(100);

                t.Property(a => a.BuildingNumber)
                .HasColumnName("BuildingNumber");
            });

            builder.HasIndex(u => u.Email).IsUnique().HasFilter("[IsDeleted] = 0"); ;
            builder.HasIndex(u => u.Phone).IsUnique().HasFilter("[IsDeleted] = 0"); ;


            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_User_Phone",
                    "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%'");
            });

            builder.HasDiscriminator<string>("UserType")
                .HasValue<Member>("Member")
                .HasValue<Trainer>("Trainer");

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }



}

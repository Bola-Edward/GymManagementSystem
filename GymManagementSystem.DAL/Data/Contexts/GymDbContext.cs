using GymManagementSystem.DAL.Data.Identity;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Data.Contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public GymDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly containing GymDbContext
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> Memberships { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}

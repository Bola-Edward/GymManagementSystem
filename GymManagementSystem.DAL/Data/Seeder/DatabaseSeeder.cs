using GymManagementSystem.DAL.Data.Identity;
using GymManagementSystem.DAL.Data.Seeder;
using GymManagementSystem.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymManagementSystem.Data.Seeder
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAllAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, GymDbContext dbContext)
        {
            await IdentitySeeder.SeedAsync(userManager, roleManager, configuration);
            await PlanSeeder.SeedAsync(dbContext);
            await CategorySeeder.SeedAsync(dbContext);

        }
    }
}

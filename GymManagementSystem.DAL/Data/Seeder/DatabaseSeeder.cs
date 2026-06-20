using GymManagementSystem.Data.Contexts;

namespace GymManagementSystem.Data.Seeder
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAllAsync(GymDbContext dbContext)
        {
            await PlanSeeder.SeedAsync(dbContext);
        }
    }
}

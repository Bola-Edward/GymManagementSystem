using GymManagementSystem.DAL.Models;
using GymManagementSystem.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Data.Seeder
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return; // Categories already seeded
            }

            var categories = new List<Category>
            {
                new Category { Name = "Yoga" },
                new Category { Name = "Cardio" },
                new Category { Name = "Strength Training" },
                new Category { Name = "CrossFit" },
                new Category { Name = "Boxing" }
            };

            await dbContext.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}

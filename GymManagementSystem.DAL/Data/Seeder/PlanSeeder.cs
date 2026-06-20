using GymManagementSystem.Data.Contexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Data.Seeder
{
    public static class PlanSeeder
    {
        public static async Task SeedAsync(GymDbContext dbContext)
        {


            bool hasAnyPlans = await dbContext.Plans.AnyAsync();

            if (hasAnyPlans)
            {
                return;
            }

            List<Plan> plans = new List<Plan>()
            {
                new Plan()
                {
                    Name = "Basic",
                    Description = "Basic gym access",
                    DurationDays = 30,
                    Price = 300,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },

                new Plan()
                {
                    Name = "Silver",
                    Description = "Gym access with cardio classes",
                    DurationDays = 60,
                    Price = 500,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },

                new Plan()
                {
                    Name = "Gold",
                    Description = "Full Gym access with trainer",
                    DurationDays = 90,
                    Price = 900,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                }


            };

            // AddRangeAsync is used to add multiple entities to the context in a single call,
            // which can be more efficient than adding them one by one using AddAsync.
            // It reduces the number of database round-trips and can improve performance when seeding large amounts of data.
            await dbContext.Plans.AddRangeAsync(plans);
            // SaveChangesAsync is used to persist the changes made to the context to the database.
            await dbContext.SaveChangesAsync();
        }
    }
}

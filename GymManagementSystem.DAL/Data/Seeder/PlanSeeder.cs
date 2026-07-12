using GymManagementSystem.DAL.Data.Seeder;
using GymManagementSystem.DAL.Data.Seeder.Models;
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

            var seederPlans = SeedJsonLoader.LoadSeedData<PlanSeedModel>("plans.json");


            if (seederPlans != null && seederPlans.Any())
            {

                var plansEntities = seederPlans.Select(model => new Plan
                {
                    Name = model.Name,
                    Price = model.Price,
                    DurationDays = model.DurationDays,
                    IsActive = model.IsActive,
                    CreatedAt = System.DateTime.Now
                }).ToList();


                await dbContext.Plans.AddRangeAsync(plansEntities);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

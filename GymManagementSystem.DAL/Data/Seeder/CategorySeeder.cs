using GymManagementSystem.DAL.Data.Seeder.Models;
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
                return;
            }

            var seederCategoryModels = SeedJsonLoader.LoadSeedData<CategorySeedModel>("categories.json");

            if (seederCategoryModels != null && seederCategoryModels.Any())
            {

                var categoriesEntities = seederCategoryModels.Select(model => new Category
                {
                    Name = model.Name

                }).ToList();


                await dbContext.Categories.AddRangeAsync(categoriesEntities);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

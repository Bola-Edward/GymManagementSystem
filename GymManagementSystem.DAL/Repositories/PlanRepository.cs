using GymManagementSystem.Data.Contexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DataAccess.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Plan plan)
        {
            _dbContext.Add(plan);
        }

        public void Delete(Plan plan)
        {
            _dbContext.Remove(plan);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await _dbContext.Plans.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _dbContext.Plans
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Plan plan)
        {
            _dbContext.Update(plan);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }
}

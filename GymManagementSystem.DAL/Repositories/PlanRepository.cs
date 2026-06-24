using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.Data.Contexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DataAccess.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {

        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {

        }



    }
}

using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {

        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {

        }



    }
}

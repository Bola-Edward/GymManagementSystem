using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.Data.Contexts;


namespace GymManagementSystem.DataAccess.Repositories
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}

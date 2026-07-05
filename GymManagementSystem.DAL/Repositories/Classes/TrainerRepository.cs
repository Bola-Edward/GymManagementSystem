using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;


namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class TrainerRepository : Repository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}

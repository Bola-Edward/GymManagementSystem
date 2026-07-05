using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class MembershipRepository : Repository<MemberShip>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}

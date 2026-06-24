using GymManagementSystem.DAL.Models;
using GymManagementSystem.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}

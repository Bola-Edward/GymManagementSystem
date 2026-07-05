using GymManagementSystem.DAL.Models;
using GymManagementSystem.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Member?> GetWithMembershipsAsync(int id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Member>()
                .AsNoTracking()
                .Include(m => m.Memberships)
                .ThenInclude(ms => ms.Plan)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public Task<bool> IsEmailTakenAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Member>().AnyAsync(m => m.Email == normalizedEmail, cancellationToken);
        }

        public Task<bool> IsPhoneTakenAsync(string phone, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Member>().AnyAsync(m => m.Phone == phone, cancellationToken);
        }
    }
}

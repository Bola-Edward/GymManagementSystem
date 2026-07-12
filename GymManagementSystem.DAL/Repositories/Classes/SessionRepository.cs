using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Session> query = _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(cancellationToken);
        }


        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken cancellationToken = default)
            => _dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, cancellationToken);

        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken cancellationToken = default)
            => _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
    }
}

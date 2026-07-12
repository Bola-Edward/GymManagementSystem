using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {

        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(
      Expression<Func<Session, bool>>? predicate = null,
      CancellationToken cancellationToken = default);

        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken cancellationToken = default);

        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken cancellationToken = default);
    }
}

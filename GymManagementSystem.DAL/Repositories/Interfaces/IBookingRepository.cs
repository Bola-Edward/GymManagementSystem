using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);

    }
}

using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IMemberRepository Members { get; }
        ITrainerRepository Trainers { get; }
        IPlanRepository Plans { get; }
        ISessionRepository Sessions { get; }
        IBookingRepository Bookings { get; }
        IMembershipRepository Memberships { get; }

        IRepository<Category> Categories { get; }
        IRepository<HealthRecord> HealthRecords { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;


        public IMemberRepository Members { get; }
        public ITrainerRepository Trainers { get; }
        public IPlanRepository Plans { get; }
        public ISessionRepository Sessions { get; }
        public IBookingRepository Bookings { get; }
        public IMembershipRepository Memberships { get; }



        public IRepository<Category> Categories { get; }
        public IRepository<HealthRecord> HealthRecords { get; }


        public UnitOfWork(GymDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));


            Members = new MemberRepository(_dbContext);
            Trainers = new TrainerRepository(_dbContext);
            Plans = new PlanRepository(_dbContext);
            Sessions = new SessionRepository(_dbContext);


            Bookings = new BookingRepository(_dbContext);
            Memberships = new MembershipRepository(_dbContext);
            Categories = new Repository<Category>(_dbContext);
            HealthRecords = new Repository<HealthRecord>(_dbContext);
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async ValueTask DisposeAsync()
        {
            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
            }
        }
    }
}

using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
    }
}

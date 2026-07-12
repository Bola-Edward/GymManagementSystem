using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId, cancellationToken);
            if (session is null) return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot cancel a booking for a session that has already started.");

            var booking = await _unitOfWork.Bookings.FindAsync(b => b.SessionId == sessionId && b.MemberId == memberId, cancellationToken);
            if (booking is null) return Result.NotFound("Booking not found.");

            await _unitOfWork.Bookings.SoftDeleteAsync(booking, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Booking Cancel Failed");
        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.FindAsync(b => b.MemberId == memberId && b.SessionId == sessionId, cancellationToken);
            if (booking is null) return Result.NotFound("Booking not found.");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;
            _unitOfWork.Bookings.Update(booking);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Mark As Attended");
        }

        public async Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(model.SessionId, cancellationToken);
            if (session is null) return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot book a session that has already started.");

            var hasActiveMembership = await _unitOfWork.Memberships.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, cancellationToken);
            if (!hasActiveMembership)
                return Result.Fail("Member does not have an active membership.");

            var alreadyBooked = await _unitOfWork.Bookings
                .AnyAsync(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId, cancellationToken);
            if (alreadyBooked)
                return Result.Fail("Member is already booked for this session.");

            var booked = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(model.SessionId, cancellationToken);
            if (booked >= session.Capacity)
                return Result.Fail("Session is full.");


            var bookingEntity = _mapper.Map<Booking>(model);

            await _unitOfWork.Bookings.AddAsync(bookingEntity, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Book Session");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
        {
            var bookings = await _unitOfWork.Sessions.GetAllSessionsWithTrainerAndCategoryAsync(x => x.EndDate >= DateTime.Now);
            if (!bookings.Any()) return null!;

            var MappedSession = _mapper.Map<IEnumerable<SessionViewModel>>(bookings);
            foreach (var item in MappedSession)
            {

                item.BookedCount = item.Capacity - await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(item.Id, cancellationToken);
            }
            return MappedSession;
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var bookings = await _unitOfWork.Bookings.GetBySessionIdAsync(sessionId, cancellationToken);


            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var bookings = await _unitOfWork.Bookings.GetBySessionIdAsync(sessionId, cancellationToken);


            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var allBookings = await _unitOfWork.Bookings.GetAllAsync(cancellationToken);
            var bookedMemberIds = allBookings.Where(x => x.SessionId == sessionId).Select(x => x.MemberId).ToList();

            var allMemberships = await _unitOfWork.Memberships.GetAllIncludingAsync(cancellationToken, m => m.Member);

            var availableMembers = allMemberships
                .Where(x => x.EndDate > DateTime.Now && !bookedMemberIds.Contains(x.MemberId))
                .Select(x => x.Member)
                .ToList();

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembers);
        }
    }
}

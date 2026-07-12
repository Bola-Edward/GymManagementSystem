using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken cancellationToken = default);

        Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken cancellationToken = default);
        Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);
        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);
    }
}

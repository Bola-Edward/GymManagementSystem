using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.HomeViewModels;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<HomeDashboardViewModel>> GetDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var now = DateTime.Now;

                var upcomingSessions = await _unitOfWork.Sessions.CountAsync(s => s.StartDate > now, cancellationToken);
                var ongoingSessions = await _unitOfWork.Sessions.CountAsync(x => x.StartDate <= now && x.EndDate >= now, cancellationToken);
                var completedSessions = await _unitOfWork.Sessions.CountAsync(x => x.EndDate < now, cancellationToken);

                var totalMembers = await _unitOfWork.Members.CountAsync(cancellationToken: cancellationToken);
                var totalTrainers = await _unitOfWork.Trainers.CountAsync(cancellationToken: cancellationToken);
                var activeMembers = await _unitOfWork.Members.CountAsync(
                    m => m.Memberships.Any(ms => ms.StartDate <= now && ms.EndDate >= now),
                    cancellationToken);


                var dashboardData = new HomeDashboardViewModel()
                {
                    TotalMembers = totalMembers,
                    TotalTrainers = totalTrainers,
                    ActiveMembers = activeMembers,
                    UpcomingSessions = upcomingSessions,
                    OngoingSessions = ongoingSessions,
                    CompletedSessions = completedSessions
                };

                return Result<HomeDashboardViewModel>.Ok(dashboardData);
            }
            catch (Exception ex)
            {
                return Result<HomeDashboardViewModel>.Fail("An unexpected error occurred while loading dashboard statistics.");
            }
        }
    }
}

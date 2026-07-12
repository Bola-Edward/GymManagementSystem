using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<HomeDashboardViewModel>> GetDashboardDataAsync(CancellationToken cancellationToken = default);
    }
}

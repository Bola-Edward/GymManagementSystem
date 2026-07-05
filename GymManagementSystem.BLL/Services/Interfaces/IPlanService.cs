using GymManagementSystem.BusinessLogic.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default);
        Task<EditPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);

        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default);
        Task<bool> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken ct = default);
    }
}

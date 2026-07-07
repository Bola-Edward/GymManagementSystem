using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;


namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken ct = default);
        Task<Result<PlanViewModel>> GetPlanByIdAsync(int planId, CancellationToken ct = default);
        Task<Result<EditPlanViewModel>> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);

        Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default);
        Task<Result> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken ct = default);
    }
}

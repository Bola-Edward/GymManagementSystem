using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;


namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken cancellationToken = default)
        {
            var plans = await _unitOfWork.Plans.GetAllAsync(cancellationToken);

            var viewModels = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationDays,
                Description = p.Description,
                IsActive = p.IsActive
            });

            return Result<IEnumerable<PlanViewModel>>.Ok(viewModels);
        }

        public async Task<Result<PlanViewModel>> GetPlanByIdAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(planId, cancellationToken);
            if (plan is null)
                return Result<PlanViewModel>.NotFound($"Plan with ID {planId} was not found.");

            var viewModel = new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                Description = plan.Description,
                IsActive = plan.IsActive
            };

            return Result<PlanViewModel>.Ok(viewModel);
        }

        public async Task<Result<EditPlanViewModel>> GetPlanToUpdateAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(planId, cancellationToken);

            if (plan is null)
                return Result<EditPlanViewModel>.NotFound($"Plan with ID {planId} was not found.");

            if (!plan.IsActive)
                return Result<EditPlanViewModel>.Fail("Cannot edit an inactive plan.", ResultKind.ValidationFailed);

            var hasActiveMemberships = await _unitOfWork.Memberships.AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.UtcNow,
                cancellationToken
            );

            if (hasActiveMemberships)
                return Result<EditPlanViewModel>.Fail("Cannot update this plan because it has active member subscriptions.", ResultKind.Conflict);

            var editModel = new EditPlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description
            };

            return Result<EditPlanViewModel>.Ok(editModel);
        }

        public async Task<Result> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken cancellationToken = default)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(id, cancellationToken);
            if (plan is null)
                return Result.Fail("Plan not found.", ResultKind.NotFound);

            var today = DateTime.UtcNow;
            var hasActiveMemberships = await _unitOfWork.Memberships.AnyAsync(
                ms => ms.PlanId == id && ms.StartDate <= today && ms.EndDate >= today,
                cancellationToken
            );

            if (hasActiveMemberships)
                return Result.Fail("Cannot update plan properties while it has active subscriptions running.", ResultKind.Conflict);

            plan.Price = model.Price;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Plans.Update(plan);
            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rowsAffected > 0 ? Result.Ok() : Result.Fail("No changes were saved.", ResultKind.Conflict);
        }

        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(planId, cancellationToken);
            if (plan is null)
                return Result.Fail("Plan not found.", ResultKind.NotFound);

            if (plan.IsActive)
            {
                var today = DateTime.UtcNow;
                var hasActiveMemberships = await _unitOfWork.Memberships.AnyAsync(
                    ms => ms.PlanId == planId && ms.StartDate <= today && ms.EndDate >= today,
                    cancellationToken
                );

                if (hasActiveMemberships)
                    return Result.Fail("Cannot deactivate this plan because members are currently subscribed to it.", ResultKind.Conflict);
            }

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Plans.Update(plan);
            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Operation failed.", ResultKind.Conflict);
        }
    }
}

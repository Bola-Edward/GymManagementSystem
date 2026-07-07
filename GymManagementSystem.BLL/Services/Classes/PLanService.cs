using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken cancellationToken = default)
        {
            var plans = await _unitOfWork.Plans.GetAllAsync(cancellationToken);


            var viewModels = _mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return Result<IEnumerable<PlanViewModel>>.Ok(viewModels);
        }

        public async Task<Result<PlanViewModel>> GetPlanByIdAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _unitOfWork.Plans.GetByIdAsync(planId, cancellationToken);
            if (plan is null)
                return Result<PlanViewModel>.NotFound($"Plan with ID {planId} was not found.");


            var viewModel = _mapper.Map<PlanViewModel>(plan);

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

            var editModel = _mapper.Map<EditPlanViewModel>(plan);

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


            _mapper.Map(model, plan);
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

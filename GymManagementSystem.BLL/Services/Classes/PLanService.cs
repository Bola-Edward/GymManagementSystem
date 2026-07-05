using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.Models;


namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<MemberShip> _membershipRepository;

        public PlanService(IRepository<Plan> planRepository, IRepository<MemberShip> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }


        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken cancellationToken = default)
        {
            var plans = await _planRepository.GetAllAsync(cancellationToken: cancellationToken);

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationDays,
                Description = p.Description,
                IsActive = p.IsActive
            });
        }


        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, cancellationToken);
            if (plan is null) return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                Description = plan.Description,
                IsActive = plan.IsActive
            };
        }


        public async Task<EditPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, cancellationToken);


            if (plan is null || !plan.IsActive) return null;


            if (await _membershipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, cancellationToken))
                return null;

            return new EditPlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description
            };
        }


        public async Task<bool> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken cancellationToken = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
            if (plan is null) return false;


            var today = DateTime.UtcNow.Date;
            var hasActiveMemberships = await _membershipRepository.AnyAsync(
                ms => ms.PlanId == id && ms.StartDate <= today && ms.EndDate >= today,
                cancellationToken
            );

            if (hasActiveMemberships)
                return false;

            plan.Price = model.Price;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.UtcNow;

            _planRepository.Update(plan);
            var rowsAffected = await _planRepository.SaveChangesAsync(cancellationToken);
            return rowsAffected > 0;
        }


        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken cancellationToken = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, cancellationToken);
            if (plan is null) return false;


            if (plan.IsActive)
            {
                var today = DateTime.UtcNow.Date;
                var hasActiveMemberships = await _membershipRepository.AnyAsync(
                    ms => ms.PlanId == planId && ms.StartDate <= today && ms.EndDate >= today,
                    cancellationToken
                );

                if (hasActiveMemberships)
                    return false;
            }


            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            _planRepository.Update(plan);
            var rowsAffected = await _planRepository.SaveChangesAsync(cancellationToken);
            return rowsAffected > 0;
        }
    }
}

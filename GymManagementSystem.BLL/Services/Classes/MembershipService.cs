using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken cancellationToken = default)
        {
            var memberExists = await _unitOfWork.Members.AnyAsync(m => m.Id == model.MemberId, cancellationToken);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await _unitOfWork.Plans.GetByIdAsync(model.PlanId, cancellationToken);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Plan is not active.");

            var hasActive = await _unitOfWork.Memberships
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, cancellationToken);
            if (hasActive) return Result.Fail("Member already has an active membership.");

            var entity = _mapper.Map<MemberShip>(model);

            entity.CreatedAt = DateTime.Now;
            entity.StartDate = model.StartDate ?? DateTime.Now;
            entity.EndDate = entity.StartDate.AddDays(plan.DurationDays);

            await _unitOfWork.Memberships.AddAsync(entity, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create New Membership");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default)
        {
            var active = await _unitOfWork.Memberships.FindAsync(
            m => m.MemberId == memberId && m.EndDate > DateTime.Now,
            cancellationToken);

            if (active is null) return Result.NotFound("No active membership for this member.");

            await _unitOfWork.Memberships.SoftDeleteAsync(active, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Membership");
        }

        public async Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken cancellationToken = default)
        {
            var memberships = await _unitOfWork.Memberships
                .GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, cancellationToken);

            return _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken cancellationToken = default)
        {
            var plans = await _unitOfWork.Plans.GetAllAsync(cancellationToken);
            var activePlans = plans.Where(p => p.IsActive);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(activePlans);
        }


        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken cancellationToken = default)
        {
            var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

    }
}

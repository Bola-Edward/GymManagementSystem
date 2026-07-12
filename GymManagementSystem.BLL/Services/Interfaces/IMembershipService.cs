using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken cancellationToken = default);
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken cancellationToken = default);
        Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default);

    }
}

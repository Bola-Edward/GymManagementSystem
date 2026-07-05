using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<bool> CreateAsync(CreateMemberViewModel model, CancellationToken cancellationToken);

        Task<MemberDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken);

        Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken cancellationToken = default);

        Task<EditMemberViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(int id, EditMemberViewModel model, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);


    }
}

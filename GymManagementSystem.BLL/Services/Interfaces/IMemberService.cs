using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.Interfaces
{
    public interface IMemberService
    {
        Task<Result<IEnumerable<MemberViewModel>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<MemberDetailsViewModel>> GetDetailsAsync(int id, CancellationToken cancellationToken);
        Task<Result<HealthRecordViewModel>> GetHealthRecordAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<EditMemberViewModel>> GetForEditAsync(int id, CancellationToken cancellationToken = default);

        Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id, EditMemberViewModel model, CancellationToken cancellationToken = default);
        Task<Result> RemoveAsync(int id, CancellationToken cancellationToken = default);


    }
}

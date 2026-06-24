using GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.Interfaces
{
    public interface IMemberService
    {
        public Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<bool> CreateAsync(CreateMemberViewModel model, CancellationToken cancellationToken);


    }
}

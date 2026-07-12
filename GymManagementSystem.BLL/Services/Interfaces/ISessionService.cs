using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
        public Task<Result<SessionViewModel>> GetSessionByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken cancellationToken = default);
        Task<EditSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken cancellationToken = default);
        Task<Result> UpdateSessionAsync(int id, EditSessionViewModel model, CancellationToken cancellationToken = default);
        Task<Result<DeleteSessionViewModel>> GetSessionToDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

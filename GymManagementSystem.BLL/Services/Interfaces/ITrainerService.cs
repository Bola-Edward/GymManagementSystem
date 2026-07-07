using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;


namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken cancellationToken = default);
        Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int trainerId, CancellationToken cancellationToken = default);
        Task<Result<EditTrainerViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken = default);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<Result> UpdateTrainerDetailsAsync(int trainerId, EditTrainerViewModel model, CancellationToken ct = default);
        Task<Result> RemoveTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}

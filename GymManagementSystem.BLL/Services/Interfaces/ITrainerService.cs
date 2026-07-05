using GymManagementSystem.BusinessLogic.ViewModels.TrainerViewModels;


namespace GymManagementSystem.BusinessLogic.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken cancellationToken = default);
        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken cancellationToken = default);
        Task<EditTrainerViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken = default);
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken cancellationToken = default);
        Task<bool> UpdateTrainerDetailsAsync(int trainerId, EditTrainerViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken cancellationToken = default);
    }
}

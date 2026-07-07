using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken cancellationToken = default)
        {
            var trainers = await _unitOfWork.Trainers.GetAllAsync(cancellationToken);


            var viewModels = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

            return Result<IEnumerable<TrainerViewModel>>.Ok(viewModels);
        }

        public async Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result<TrainerViewModel>.NotFound($"Trainer with ID {trainerId} was not found.");


            var viewModel = _mapper.Map<TrainerViewModel>(trainer);

            return Result<TrainerViewModel>.Ok(viewModel);
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.Trainers.AnyAsync(t => t.Phone == model.Phone, cancellationToken))
                return Result.Fail("Trainer with this phone number already exists.", ResultKind.Conflict);


            var trainer = _mapper.Map<Trainer>(model);
            trainer.HireDate = DateTime.UtcNow;

            await _unitOfWork.Trainers.AddAsync(trainer, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create trainer.", ResultKind.Conflict);
        }

        public async Task<Result<EditTrainerViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result<EditTrainerViewModel>.NotFound($"Trainer with ID {trainerId} was not found.");


            var editModel = _mapper.Map<EditTrainerViewModel>(trainer);

            return Result<EditTrainerViewModel>.Ok(editModel);
        }

        public async Task<Result> UpdateTrainerDetailsAsync(int trainerId, EditTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result.Fail("Trainer not found.", ResultKind.NotFound);

            if (await _unitOfWork.Trainers.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, cancellationToken))
                return Result.Fail("Trainer with this phone number already exists.", ResultKind.Conflict);


            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Trainers.Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("No changes were saved.", ResultKind.Conflict);
        }

        public async Task<Result> RemoveTrainerAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result.Fail("Trainer not found.", ResultKind.NotFound);

            var hasFutureSessions = await _unitOfWork.Sessions
                .AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.UtcNow, cancellationToken);

            if (hasFutureSessions)
                return Result.Fail("Cannot remove trainer because they have active future sessions.", ResultKind.Conflict);

            await _unitOfWork.Trainers.SoftDeleteAsync(trainer, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to remove trainer.", ResultKind.Conflict);
        }
    }
}

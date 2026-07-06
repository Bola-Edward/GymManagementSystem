using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken cancellationToken = default)
        {
            var trainers = await _unitOfWork.Trainers.GetAllAsync(cancellationToken);

            var viewModels = trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Speciality = t.Speciality.ToString(),
                Phone = t.Phone,
                Email = t.Email,
            });

            return Result<IEnumerable<TrainerViewModel>>.Ok(viewModels);
        }


        public async Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result<TrainerViewModel>.NotFound($"Trainer with ID {trainerId} was not found.");

            var viewModel = new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Speciality = trainer.Speciality.ToString(),
                Phone = trainer.Phone,
                Email = trainer.Email,
                Address = trainer.Address != null ? $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}" : "No Address",
                DateOfBirth = trainer.DateOfBirth.ToString("MM/dd/yyyy")
            };

            return Result<TrainerViewModel>.Ok(viewModel);
        }


        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.Trainers.AnyAsync(t => t.Phone == model.Phone, cancellationToken))
                return Result.Fail("Trainer with this phone number already exists.", ResultKind.Conflict);

            var trainer = new Trainer
            {
                Name = model.Name,
                Speciality = model.Speciality,
                Phone = model.Phone,
                Email = model.Email,
                HireDate = DateTime.UtcNow,
                DateOfBirth = model.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                }
            };

            await _unitOfWork.Trainers.AddAsync(trainer, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create trainer.", ResultKind.Conflict);
        }


        public async Task<Result<EditTrainerViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result<EditTrainerViewModel>.NotFound($"Trainer with ID {trainerId} was not found.");

            var editModel = new EditTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Speciality = trainer.Speciality,
                Phone = trainer.Phone,
                Email = trainer.Email,
            };

            return Result<EditTrainerViewModel>.Ok(editModel);
        }


        public async Task<Result> UpdateTrainerDetailsAsync(int trainerId, EditTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null)
                return Result.Fail("Trainer not found.", ResultKind.NotFound);

            if (await _unitOfWork.Trainers.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, cancellationToken))
                return Result.Fail("Trainer with this phone number already exists.", ResultKind.Conflict);

            trainer.Name = model.Name;
            trainer.Speciality = model.Speciality;
            trainer.Phone = model.Phone;
            trainer.Email = model.Email;
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

using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;
using GymManagementSystem.DAL.Repositories;


namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IRepository<Trainer> _trainerRepository;
        private readonly IRepository<Session> _sessionRepository;

        public TrainerService(IRepository<Trainer> trainerRepository, IRepository<Session> sessionRepository)
        {
            _trainerRepository = trainerRepository;
            _sessionRepository = sessionRepository;
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken cancellationToken = default)
        {
            var trainers = await _trainerRepository.GetAllAsync(cancellationToken);

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Speciality = t.Speciality.ToString(),
                Phone = t.Phone,
                Email = t.Email,
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null) return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Speciality = trainer.Speciality.ToString(),
                Phone = trainer.Phone,
                Email = trainer.Email,
                Address = trainer.Address != null ? $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}" : "No Address",
                DateOfBirth = trainer.DateOfBirth.ToString("MM/dd/yyyy")
            };
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            if (await _trainerRepository.AnyAsync(t => t.Phone == model.Phone, cancellationToken))
                return false;

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

            await _trainerRepository.AddAsync(trainer, cancellationToken);
            var result = await _trainerRepository.SaveChangesAsync(cancellationToken);
            return result > 0;
        }


        public async Task<EditTrainerViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null) return null;

            return new EditTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Speciality = trainer.Speciality,
                Phone = trainer.Phone,
                Email = trainer.Email,
            };
        }


        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, EditTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null) return false;

            if (await _trainerRepository.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, cancellationToken))
                return false;

            trainer.Name = model.Name;
            trainer.Speciality = model.Speciality;
            trainer.Phone = model.Phone;
            trainer.Email = model.Email;
            trainer.UpdatedAt = DateTime.UtcNow;

            _trainerRepository.Update(trainer);
            var result = await _trainerRepository.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken cancellationToken = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, cancellationToken);
            if (trainer is null) return false;

            var hasFutureSessions = await _sessionRepository
                .AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.UtcNow, cancellationToken);

            if (hasFutureSessions) return false;

            await _trainerRepository.SoftDeleteAsync(trainer, cancellationToken);
            var result = await _trainerRepository.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}

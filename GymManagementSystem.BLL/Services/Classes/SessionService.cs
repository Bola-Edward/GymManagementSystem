using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Enums;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
        {

            var sessions = await _unitOfWork.Sessions.GetAllSessionsWithTrainerAndCategoryAsync(null, cancellationToken);

            if (sessions == null || !sessions.Any())
                return Result<IEnumerable<SessionViewModel>>.NotFound("No sessions found.");


            sessions = sessions.OrderByDescending(x => x.StartDate);


            var viewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);


            foreach (var vm in viewModels)
            {
                var bookedSlots = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(vm.Id, cancellationToken);
                vm.BookedCount = bookedSlots;

            }

            return Result<IEnumerable<SessionViewModel>>.Ok(viewModels);
        }

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.Sessions.GetSessionWithTrainerAndCategoryAsync(id, cancellationToken);

            if (session is null)
                return Result<SessionViewModel>.NotFound($"Session with ID {id} was not found.");

            var viewModel = _mapper.Map<SessionViewModel>(session);


            viewModel.BookedCount = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(id, cancellationToken);

            return Result<SessionViewModel>.Ok(viewModel);
        }


        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken cancellationToken = default)
        {

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date.");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future.");


            var trainer = await _unitOfWork.Trainers.GetByIdAsync(model.TrainerId, cancellationToken);
            if (trainer is null)
                return Result.NotFound("Trainer not found.");


            var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId, cancellationToken);
            if (category is null)
                return Result.NotFound("Category not found.");


            var isValidSpecialty = Enum.TryParse<Speciality>(category.Name, true, out var categorySpecialty);

            if (!isValidSpecialty || trainer.Speciality != categorySpecialty)
                return Result.Validation("Cannot create this session for this trainer due to specialty mismatch.");


            var session = _mapper.Map<Session>(model);


            await _unitOfWork.Sessions.AddAsync(session, cancellationToken);
            var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to create session.");
        }


        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken cancellationToken = default)
        {
            var trainers = await _unitOfWork.Trainers.GetAllAsync(cancellationToken: cancellationToken);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken: cancellationToken);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<EditSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId, cancellationToken);
            if (session is null) return null;
            if (!await IsSessionValidForUpdatingAsync(session, cancellationToken)) return null;
            return _mapper.Map<EditSessionViewModel>(session);
        }

        private async Task<bool> IsSessionValidForUpdatingAsync(Session session, CancellationToken cancellationToken = default)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var booked = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(session.Id, cancellationToken);
            return booked == 0;
        }

        public async Task<Result> UpdateSessionAsync(int id, EditSessionViewModel model, CancellationToken cancellationToken = default)
        {
            var sessionRepo = _unitOfWork.Sessions;
            var session = await sessionRepo.GetByIdAsync(id, cancellationToken);

            if (session is null)
                return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot edit a session that has already started.");

            var bookedCount = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(id, cancellationToken);
            if (bookedCount > 0)
                return Result.Fail("Cannot edit a session that already has bookings.");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date.");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future.");

            var trainerRepo = _unitOfWork.Trainers;
            var trainer = await trainerRepo.GetByIdAsync(model.TrainerId, cancellationToken);
            if (trainer is null)
                return Result.NotFound("Trainer not found.");

            var categoryRepo = _unitOfWork.Categories;


            var category = await categoryRepo.GetByIdAsync(model.CategoryId, cancellationToken);
            if (category is null)
                return Result.NotFound("Category not found.");

            var isValidSpecialty = Enum.TryParse<Speciality>(category.Name, true, out var categorySpecialty);
            if (!isValidSpecialty || trainer.Speciality != categorySpecialty)
            {
                return Result.Validation("This trainer does not match the session category.");
            }

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;

            sessionRepo.Update(session);
            var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed to update session.");
        }


        public async Task<Result<DeleteSessionViewModel>> GetSessionToDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.Sessions.GetSessionWithTrainerAndCategoryAsync(id, cancellationToken);

            if (session is null)
                return Result<DeleteSessionViewModel>.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result<DeleteSessionViewModel>.Fail("Cannot delete a session that has already started.");

            var bookedCount = await _unitOfWork.Sessions.GetCountOfBookedSlotsAsync(id, cancellationToken);
            if (bookedCount > 0)
                return Result<DeleteSessionViewModel>.Fail("Cannot delete a session that already has bookings.");

            var model = _mapper.Map<DeleteSessionViewModel>(session);

            return Result<DeleteSessionViewModel>.Ok(model);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var sessionRepo = _unitOfWork.Sessions;


            var session = await sessionRepo.GetByIdAsync(id, cancellationToken);
            if (session is null)
                return Result.NotFound("Session not found.");


            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot delete a session that has already started.");


            var bookedCount = await sessionRepo.GetCountOfBookedSlotsAsync(id, cancellationToken);
            if (bookedCount > 0)
                return Result.Fail("Cannot delete a session that already has bookings.");


            await sessionRepo.SoftDeleteAsync(session, cancellationToken);

            var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete the session.");
        }
    }
}

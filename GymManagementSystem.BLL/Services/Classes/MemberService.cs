using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MemberViewModel>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);


            var viewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return Result<IEnumerable<MemberViewModel>>.Ok(viewModels);
        }

        public async Task<Result<MemberDetailsViewModel>> GetDetailsAsync(int id, CancellationToken cancellationToken)
        {
            var member = await _unitOfWork.Members.GetWithMembershipsAsync(id, cancellationToken);

            if (member == null)
                return Result<MemberDetailsViewModel>.NotFound($"Member with ID {id} was not found.");


            var viewModel = _mapper.Map<MemberDetailsViewModel>(member);


            var today = DateTime.UtcNow;
            var activeMembership = member.Memberships.FirstOrDefault(m => m.StartDate <= today && m.EndDate >= today);

            viewModel.PLanName = activeMembership?.Plan.Name ?? "No Active Plan";
            viewModel.MembershipStartDate = activeMembership?.StartDate.ToShortDateString() ?? "-";
            viewModel.MembershipEndDate = activeMembership?.EndDate.ToShortDateString() ?? "-";

            return Result<MemberDetailsViewModel>.Ok(viewModel);
        }

        public async Task<Result<EditMemberViewModel>> GetForEditAsync(int id, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null)
                return Result<EditMemberViewModel>.NotFound($"Member with ID {id} was not found.");


            var editModel = _mapper.Map<EditMemberViewModel>(member);

            return Result<EditMemberViewModel>.Ok(editModel);
        }

        public async Task<Result<HealthRecordViewModel>> GetHealthRecordAsync(int id, CancellationToken cancellationToken = default)
        {
            var record = await _unitOfWork.HealthRecords.FindAsync(x => x.MemberId == id, cancellationToken);
            if (record is null)
                return Result<HealthRecordViewModel>.NotFound("Health record not found for this member.");


            var viewModel = _mapper.Map<HealthRecordViewModel>(record);

            return Result<HealthRecordViewModel>.Ok(viewModel);
        }

        public async Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.Members.IsEmailTakenAsync(model.Email, ct);
            var phoneExists = await _unitOfWork.Members.IsPhoneTakenAsync(model.Phone, ct);

            if (emailExists) return Result.Fail("This email address is already taken.", ResultKind.Conflict);
            if (phoneExists) return Result.Fail("This phone number is already registered.", ResultKind.Conflict);


            var member = _mapper.Map<Member>(model);
            member.JoinDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.Members.AddAsync(member);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Failed to create member.", ResultKind.Conflict);
        }

        public async Task<Result> UpdateAsync(int id, EditMemberViewModel model, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null) return Result.Fail("Member not found.", ResultKind.NotFound);

            if (await _unitOfWork.Members.AnyAsync(m => m.Email == model.Email && m.Id != id, cancellationToken))
                return Result.Fail("Email is already registered to another member.", ResultKind.Conflict);

            if (await _unitOfWork.Members.AnyAsync(m => m.Phone == model.Phone && m.Id != id, cancellationToken))
                return Result.Fail("Phone number is already registered to another member.", ResultKind.Conflict);


            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Members.Update(member);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Ok() : Result.Fail("No changes were saved.", ResultKind.Conflict);
        }

        public async Task<Result> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null) return Result.Fail("Member not found.", ResultKind.NotFound);

            var hasFutureSessions = await _unitOfWork.Bookings.AnyAsync(b => b.MemberId == id && b.Session.StartDate > DateTime.UtcNow);

            if (hasFutureSessions)
                return Result.Fail("Cannot remove member because they have active future bookings.", ResultKind.Conflict);

            await _unitOfWork.Members.SoftDeleteAsync(member, cancellationToken);

            if (member.HealthRecord != null)
            {
                await _unitOfWork.HealthRecords.SoftDeleteAsync(member.HealthRecord, cancellationToken);
            }

            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return rowsAffected > 0 ? Result.Ok() : Result.Fail("Failed to delete member.", ResultKind.Conflict);
        }
    }
}

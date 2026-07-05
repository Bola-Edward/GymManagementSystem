using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Enums;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;
using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            #region Loading All Members To Check [Wrong Way ]
            //var emailExists = await _memberRepository.GetAllAsync();

            // So We Need A Function in Repository To Just Check If any exists
            // Or Not With Condition And Return True Or False Without Loading Data  

            #endregion

            var emailExists = await _unitOfWork.Members.IsEmailTakenAsync(model.Email, ct);
            var phoneExists = await _unitOfWork.Members.IsPhoneTakenAsync(model.Phone, ct);

            // Return False If Any Exists 
            if (emailExists || phoneExists) return false;
            // True If Member Added
            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Notes = model.HealthRecordViewModel.Note,
                    BloodType = model.HealthRecordViewModel.BloodType,
                }
            };

            await _unitOfWork.Members.AddAsync(Member);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);

            return members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString(),
                PhotoUrl = m.Photo,
                JoinDate = m.JoinDate
            });
        }

        public async Task<MemberDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken)
        {
            // get member with its membership and plan details
            var member = await _unitOfWork.Members.GetWithMembershipsAsync(id: id, cancellationToken: cancellationToken);

            if (member == null) return null;

            var today = DateTime.UtcNow;
            var activeMembership = member.Memberships.FirstOrDefault(m => m.StartDate <= today && m.EndDate >= today);


            return new MemberDetailsViewModel
            {
                Id = member.Id,
                Name = member.Name,
                PhotoUrl = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
                PLanName = activeMembership?.Plan.Name ?? "No Active Plan",
                MembershipEndDate = activeMembership?.EndDate.ToShortDateString() ?? "-",
                MembershipStartDate = activeMembership?.StartDate.ToShortDateString() ?? "-"
            };
        }

        public async Task<EditMemberViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null)
                return null;
            else
                return new EditMemberViewModel()
                {
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Street = member.Address.Street,
                    City = member.Address.City,
                    BuildingNumber = member.Address.BuildingNumber,
                    Photo = member.Photo
                };
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken cancellationToken = default)
        {
            var record = await _unitOfWork.HealthRecords.FindAsync(x => x.MemberId == id, cancellationToken: cancellationToken);
            if (record is null) return null;

            else
                return new HealthRecordViewModel()
                {
                    Weight = record.Weight,
                    BloodType = record.BloodType,
                    Height = record.Height,
                    Note = record.Notes
                };
        }

        public async Task<bool> UpdateAsync(int id, EditMemberViewModel model, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null) return false;

            if (await _unitOfWork.Members.AnyAsync(m => m.Email == model.Email && m.Id != id, cancellationToken))
                return false;
            if (await _unitOfWork.Members.AnyAsync(m => m.Phone == model.Phone && m.Id != id, cancellationToken))
                return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;


            _unitOfWork.Members.Update(member);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? true : false;
        }

        public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id, cancellationToken);
            if (member is null) return false;


            var hasFutureSessions = await _unitOfWork.Bookings.AnyAsync(b => b.MemberId == id && b.Session.StartDate > DateTime.Now);

            if (hasFutureSessions)
                return false;

            await _unitOfWork.Members.SoftDeleteAsync(member, cancellationToken);
            await _unitOfWork.HealthRecords.SoftDeleteAsync(member.HealthRecord, cancellationToken);

            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rowsAffected > 0;
        }
    }
}

using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Enums;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;
using GymManagementSystem.DAL.Repositories;

namespace GymManagementSystem.BusinessLogic.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IRepository<Member> _memberRepository;

        public MemberService(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<bool> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            #region Loading All Members To Check [Wrong Way ]
            //var emailExists = await _memberRepository.GetAllAsync();

            // So We Need A Function in Repository To Just Check If any exists
            // Or Not With Condition And Return True Or False Without Loading Data  

            #endregion

            // Check Email Exists 
            var emailExists = await _memberRepository.AnyAsync(x => x.Email == model.Email, ct);
            // Check Phone Exists 
            var PhoneExists = await _memberRepository.AnyAsync(x => x.Phone == model.Phone, ct);

            // Return False If Any Exists 
            if (emailExists || PhoneExists) return false;
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
                    BloodType = Enum.Parse<BloodType>(model.HealthRecordViewModel.BloodType)
                }
            };

            await _memberRepository.AddAsync(Member);
            var rowsAffected = await _memberRepository.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var members = await _memberRepository.GetAllAsync(cancellationToken);

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
    }
}

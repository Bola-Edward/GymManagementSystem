using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;


namespace GymManagementSystem.BLL.Mapping
{
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {

            CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            }))
            .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel != null ? new HealthRecord
            {
                Weight = src.HealthRecordViewModel.Weight,
                Height = src.HealthRecordViewModel.Height,
                Notes = src.HealthRecordViewModel.Note,
                BloodType = src.HealthRecordViewModel.BloodType
            } : null));


            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo));


            CreateMap<Member, MemberDetailsViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    src.Address != null ? $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}" : "No Address"))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));




            CreateMap<Member, EditMemberViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ReverseMap()
                .BeforeMap((src, dest) =>
                {
                    if (dest.Address == null) dest.Address = new Address();
                })
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));


            CreateMap<EditMemberViewModel, Member>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore());


            CreateMap<HealthRecord, HealthRecordViewModel>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Notes))
                .ReverseMap()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Note));
        }
    }
}

using AutoMapper;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.ValueObjects;

namespace GymManagementSystem.BLL.Mapping
{
    public class TrainerMappingProfile : Profile
    {
        public TrainerMappingProfile()
        {

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Speciality, opt => opt.MapFrom(src => src.Speciality.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    src.Address != null ? $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}" : "No Address"));


            CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            }));


            CreateMap<Trainer, EditTrainerViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : string.Empty))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNumber : 0))
                .ReverseMap()
                .BeforeMap((src, dest) =>
                {
                    if (dest.Address == null) dest.Address = new Address();
                })
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));


        }
    }
}


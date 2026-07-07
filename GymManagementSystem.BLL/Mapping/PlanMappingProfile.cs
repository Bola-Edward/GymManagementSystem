using AutoMapper;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Mapping
{
    public class PlanMappingProfile : Profile
    {
        public PlanMappingProfile()
        {

            CreateMap<Plan, PlanViewModel>().ReverseMap();


            CreateMap<Plan, EditPlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlanName));
        }
    }
}

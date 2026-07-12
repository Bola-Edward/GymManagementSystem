using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Mapping
{
    public class MembershipMappingProfile : Profile
    {

        public MembershipMappingProfile()
        {
            var now = DateTime.Now;

            CreateMap<MemberShip, MemberShipForMemberViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))

                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(Src => Src.StartDate))

                     .ForMember(dist => dist.Status, Option => Option.MapFrom(Src => now >= Src.StartDate && now <= Src.EndDate ? "Active" : "Expired"));

            CreateMap<MemberShip, MemberShipViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(Src => Src.StartDate));

            CreateMap<CreateMemberShipViewModel, MemberShip>()
                     .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Member, MemberSelectListViewModel>();
            CreateMap<Plan, PlanSelectListViewModel>();
        }
    }
}

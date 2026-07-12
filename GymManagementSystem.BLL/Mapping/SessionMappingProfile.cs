using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
              .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer != null ? src.Trainer.Name : "No Trainer"))
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "General"))
              .ForMember(dest => dest.BookedCount, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Session, EditSessionViewModel>();
            CreateMap<EditSessionViewModel, Session>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Session, DeleteSessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd hh:mm tt")))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd hh:mm tt")));
        }
    }
}

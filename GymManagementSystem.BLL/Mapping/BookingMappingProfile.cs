using AutoMapper;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Mapping
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {

            CreateMap<CreateBookingViewModel, Booking>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));


            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd hh:mm tt")));
        }
    }
}

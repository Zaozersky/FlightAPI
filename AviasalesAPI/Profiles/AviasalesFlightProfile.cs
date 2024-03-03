using Aviasales.DAL.Entities;
using AviasalesAPI.Models;
using AutoMapper;

namespace AviasalesAPI.Profiles
{
    public class AviasalesFlightProfile : Profile
    {
        public AviasalesFlightProfile()
        {
            CreateMap<AviasalesFlight, AviasalesFlightDto>()
               .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.airline))
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.flight_number));
        }
    }
}

using AutoMapper;
using FlightAPI.Models;

namespace FlightAPI.Profiles
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<JsonFlight, FlightDto>()
                .ForMember(dest => dest.Origin,
                           opt => opt.MapFrom
                           (src => !string.IsNullOrEmpty(src.airline) ? src.airline : src.airport_from))
                .ForMember(dest => dest.Destination,
                           opt => opt.MapFrom
                           (src => !string.IsNullOrEmpty(src.destination) ? src.destination : src.airport_to))
                .ForMember(dest => dest.DepartureDate,
                           opt => opt.MapFrom
                           (src => src.departure != null ? src.departure : src.departure_date))
                .ForMember(dest => dest.ArrivalDate,
                           opt => opt.MapFrom
                           (src => src.arrival != null ? src.arrival : src.arrival_date));
        }
    }
}


using Airport.DAL;
using Airport.DAL.Entities;
using AirportAPI.Models;
using AutoMapper;

namespace AirportAPI.Profiles
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, AirportFlightDto>()
             .ForMember(f => f.FlightNumber , opt => opt.MapFrom(c => c.flight_number));
        }
    }
}


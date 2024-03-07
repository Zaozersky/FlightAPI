using AutoMapper;
using FlightAPI.Profiles;

public static class MappingProfile
{
    public static MapperConfiguration InitializeAutoMapper()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.AddProfile(new FlightProfile());
        });

        return config;
    }
}

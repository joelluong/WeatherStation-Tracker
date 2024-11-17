using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<WeatherStation, WeatherStationDto>();
        CreateMap<WeatherVariable, WeatherVariableDto>();
        CreateMap<WeatherData, WeatherDataDto>();
    }
}
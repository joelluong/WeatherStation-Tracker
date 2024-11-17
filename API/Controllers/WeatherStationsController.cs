using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/weather-stations")]
public class WeatherStationsController : ControllerBase
{
    private readonly WeatherDbContext _context;
    private readonly IMapper _mapper;

    public WeatherStationsController(WeatherDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<WeatherStationDto>>> GetAllWeatherStations()
    {
        var query = _context.WeatherStations
            .Include(ws => ws.Variables)
            .ThenInclude(v => v.Data)
            .AsQueryable();

        return await query.ProjectTo<WeatherStationDto>(_mapper.ConfigurationProvider).ToListAsync();

    }
}
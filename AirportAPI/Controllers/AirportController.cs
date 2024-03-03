using AirportAPI.Common;
using AirportAPI.Controllers.Services;
using AirportAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportController : ControllerBase
{
    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;

    /// <summary>
    /// .ctor
    /// </summary>
    public AirportController(IFlightService flightService, IMapper mapper)
    {
        _flightService = flightService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get flights from airport data source
    /// </summary>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>Airport data flights</returns>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        IEnumerable<AirportFlightDto> flights;

        try
        {
            var flightData = await _flightService.GetFlights(cancellationToken);

            if (flightData == null)
            {
                return NotFound();
            }

            flights = _mapper.Map<IEnumerable<AirportFlightDto>>(flightData);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(flights);
    }

    /// <summary>
    /// Book flight
    /// </summary>
    /// <param name="flightId">flight id</param>
    /// <param name="cancellationToken">cancellation token</param>
    [HttpPatch]
    public async Task<IActionResult> BookFlight(int flightId, CancellationToken cancellationToken)
    {
        try
        {
            await _flightService.CreateBookFlight(flightId, cancellationToken);
            return Ok();
        }
        catch (FlightServiceNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}


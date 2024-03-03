using FlightAPI.Common;
using FlightAPI.Models;
using FlightAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly ILogger<FlightsController> _logger;
    private IAggregatedFlightService _service;

    /// <summary>
    /// .ctor
    /// </summary>
    public FlightsController(IAggregatedFlightService service,
        ILogger<FlightsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Receive flights
    /// </summary>
    /// <param name="query">query parameters (airline, date, price, etc)</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns>Flights data</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var flights = await _service.GetFlights(query, cancellationToken);

            if (flights == null)
            {
                _logger.LogWarning("Flights are not found.");
                return NotFound();
            }

            _logger.LogWarning($"Count of flights: {flights.Count()}");
            return Ok(flights);

        }
        catch (Exception e)
        {
            _logger.LogError(e.StackTrace, query);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Book the flight
    /// </summary>
    /// <remarks>
    /// Request example:
    ///
    ///     PATCH api/Flights
    ///     {
    ///        "Id" : 1, 
    ///        "NameAPI" : "AirportAPI"
    ///     }
    /// </remarks>
    /// <param name="flight">flight data</param>
    /// <param name="cancellationToken">cancellation token</param>
    [HttpPatch]
    public async Task<IActionResult> BookFlight([FromBody] FlightDto flight, CancellationToken cancellationToken)
    {
        try
        {
            await _service.BookFlight(flight.Id, flight.NameAPI, cancellationToken);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.StackTrace, flight.Id, flight.NameAPI);
            return BadRequest(e.Message);
        }
    }
}
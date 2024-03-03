using Aviasales.DAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aviasales.DAL.Common;
using AviasalesAPI.Models;

namespace AviasalesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AviasalesController : ControllerBase
    {
        private readonly AviasalesDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// .ctor
        /// </summary>
        public AviasalesController(AviasalesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get flights from aviasales data source
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>Aviasales data flights</returns>
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _dbContext.AviasalesFlights.ToListAsync(cancellationToken);
                var flights = _mapper.Map<IEnumerable<AviasalesFlightDto>>(data);

                return Ok(flights);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Book the flight
        /// </summary>
        /// <param name="flightId">flight id</param>
        /// <param name="cancellationToken">cancellation token</param>
        [HttpPatch]
        public async Task<IActionResult> CreateBookFlight([FromBody] int flightId, CancellationToken cancellationToken)
        {
            try
            {
                var foundFlight = await _dbContext
                    .AviasalesFlights
                    .Where(o => o.id == flightId)
                    .SingleOrDefaultAsync(cancellationToken);

                if (foundFlight == null)
                {
                    return NotFound($"Flight with id: {flightId} not found.");
                }

                var newOrder = new AviasalesOrder
                {
                    flight_id = flightId
                };

                await _dbContext.AviasalesOrders.AddAsync(newOrder, cancellationToken);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
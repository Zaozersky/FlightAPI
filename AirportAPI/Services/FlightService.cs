using Airport.DAL.Common;
using Airport.DAL.Entities;
using AirportAPI.Common;
using Microsoft.EntityFrameworkCore;

namespace AirportAPI.Controllers.Services
{
    public class FlightService : IFlightService, IDisposable
    {
        private bool _disposed;

        private readonly AirportDbContext _dbContext;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dbContext">database context</param>
        /// <param name="memoryCache">memory cache</param>
        /// <param name="logger">logger</param>
        /// <param name="options">application settings</param>
        public FlightService(AirportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Receive flights
        /// </summary>
        /// <param name="query">query parameters (date, price, airline, etc)</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>flights</returns>
        public async Task<IEnumerable<Flight>> GetFlights(
            CancellationToken cancellationToken)
        {
            var flights = from flight in _dbContext.Flights
                          select flight;

            return await flights.ToListAsync(cancellationToken);

        }

        /// <summary>
        /// Book the flight
        /// </summary>
        /// <param name="flightId">flight id</param>
        /// <param name="cancellationToken">cancellation token</param>
        public async Task CreateBookFlight(int flightId,
            CancellationToken cancellationToken)
        {
            var foundFlight = await _dbContext
                .Flights
                .Where(o => o.id == flightId)
                .SingleOrDefaultAsync(cancellationToken);

            if (foundFlight == null)
            {
                var errorMessage = $"The flight with id: {flightId} not found.";
                throw new FlightServiceNotFoundException(errorMessage);
            }

            var newOrder = new Order
            {
                FlightId = flightId
            };

            await _dbContext.Orders.AddAsync(newOrder, cancellationToken);
            await _dbContext.SaveChangesAsync();

        }

        /// <summary>
        /// Cancel flight booking
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="cancellationToken">cancellation token</param>
        public async Task CancelBookedFlight(int orderId, CancellationToken cancellationToken)
        {
            var foundOrder = await _dbContext
                .Orders
                .Where(o => o.Id == orderId)
                .SingleOrDefaultAsync(cancellationToken);

            if (foundOrder == null)
            {
                var errorMessage = $"Order with id: {orderId} not found.";
                throw new FlightServiceNotFoundException(errorMessage);
            }

            _dbContext.Orders.Remove(foundOrder);
            await _dbContext.SaveChangesAsync();
        }

        #region Dispose methods
        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion 
    }
}
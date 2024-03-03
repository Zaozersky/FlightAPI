using Airport.DAL.Entities;
using AirportAPI.Common;

namespace AirportAPI.Controllers.Services
{
    public interface IFlightService
    {
        Task CreateBookFlight(int flightId, CancellationToken cancellationToken);
        Task CancelBookedFlight(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<Flight>> GetFlights(CancellationToken cancellationToken);
    }
}
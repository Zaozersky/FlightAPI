
using FlightAPI.Common;
using FlightAPI.Models;

namespace FlightAPI.Services
{
    public interface IAggregatedFlightService
    {
        Task<IEnumerable<FlightDto>> GetFlights(QueryParameters query, CancellationToken cancellationToken);
        Task BookFlight(int flightId, string nameAPI, CancellationToken cancellationToken);
    }
}
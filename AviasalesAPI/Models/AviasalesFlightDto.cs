namespace AviasalesAPI.Models
{
    /// <summary>
    /// Aviasales flight data
    /// </summary>
    public class AviasalesFlightDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Origin airport
        /// </summary>
        public string? Airport_from { get; set; }

        /// <summary>
        /// Destination airport
        /// </summary>
        public string? Airport_to { get; set; }

        /// <summary>
        /// Airline name
        /// </summary>
        public string? Airline { get; set; }

        /// <summary>
        /// Flight number
        /// </summary>
        public string? FlightNumber { get; set; }

        /// <summary>
        /// Departure date from <see cref="Airport_from"> airport
        /// </summary>
        public DateTime Departure_date { get; set; }

        /// <summary>
        /// Arrival date to <see cref="Airport_to"> airport
        /// </summary>
        public DateTime Arrival_date { get; set; }

        /// <summary>
        /// Transfers count
        /// </summary>
        public int TransfersCount { get; set; }

        /// <summary>
        /// Ticket price
        /// </summary>
        public decimal Price { get; set; }
    }
}
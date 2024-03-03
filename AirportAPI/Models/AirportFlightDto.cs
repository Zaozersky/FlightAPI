namespace AirportAPI.Models
{
    /// <summary>
    /// POCO for flight data
    /// </summary>
    public class AirportFlightDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Origin airport
        /// </summary>
        public string? Origin { get; set; }

        /// <summary>
        /// Destination airport
        /// </summary>
        public string? Destination { get; set; }

        /// <summary>
        /// Airline name
        /// </summary>
        public string? Airline { get; set; }

        /// <summary>
        /// Flight number
        /// </summary>
        public string? FlightNumber { get; set; }

        /// <summary>
        /// Departure date from <see cref="Origin"> airport
        /// </summary>
        public DateTime Departure { get; set; }

        /// <summary>
        /// Arrival date to <see cref="Destination"> airport
        /// </summary>
        public DateTime Arrival { get; set; }

        /// <summary>
        /// Transfers count
        /// </summary>
        public int Transfers { get; set; }

        /// <summary>
        /// Ticket price
        /// </summary>
        public decimal Price { get; set; }
    }
}
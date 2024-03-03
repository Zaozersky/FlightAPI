namespace FlightAPI.Models
{
    /// <summary>
    /// POCO for flight data
    /// </summary>
    public class FlightDto
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
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Arrival date to <see cref="Destination"> airport
        /// </summary>
        public DateTime ArrivalDate { get; set; }

        /// <summary>
        /// Transfers count
        /// </summary>
        public int Transfers { get; set; }

        /// <summary>
        /// Ticket price
        /// </summary>
        public decimal Price { get; set; }
        
        public string NameAPI { get; set; }
    }
}
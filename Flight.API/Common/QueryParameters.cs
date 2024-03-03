namespace FlightAPI.Common
{
    /// <summary>
    /// Represents query parameters for filtering and sorting
    /// </summary>
    public class QueryParameters
    {
        /// <summary>
        /// String for filtering flights by date, price, number of transfers, airline
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// String for sorting flights
        /// </summary>
        public string? Order { get; set; }

        public override string ToString()
        {
            return string.Join(".", Filter??string.Empty, Order??string.Empty);
        }
    }
}
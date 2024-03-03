using System;
namespace FlightAPI.Models
{
    public partial class JsonFlight
    {
        public int id { get; set; }
        public string? origin { get; set; }
        public string? destination { get; set; }
        public string? airline { get; set; }
        public DateTime? departure { get; set; }
        public DateTime? arrival { get; set; }
        public int? transfers { get; set; }
        public string? flightNumber { get; set; }
    }

    public partial class JsonFlight
    {
        public string? airport_from { get; set; }
        public string? airport_to { get; set; }
        public string? company { get; set; }
        public DateTime? departure_date { get; set; }
        public DateTime? arrival_date { get; set; }
        public int? transfersCount { get; set; }
        public decimal? price { get; set; }
    }
}
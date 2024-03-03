namespace Airport.DAL.Entities
{
    public class Flight
    {
        public int id { get; set; }

        public string? origin { get; set; }

        public string? destination { get; set; }

        public string? airline { get; set; }

        public DateTime? departure { get; set; }

        public DateTime? arrival { get; set; }

        public int? transfers { get; set; }

        public decimal? price { get; set; }

        public string? currency { get; set; }

        public string? flight_number { get; set; }
    }
}
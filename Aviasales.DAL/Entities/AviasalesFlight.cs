using System.ComponentModel.DataAnnotations;

namespace Aviasales.DAL.Entities
{
    public class AviasalesFlight
    {
        [Key]
        public int id { get; set; }

        public string? airport_from { get; set; }

        public string? airport_to { get; set; }

        public string? airline { get; set; }

        public DateTime? departure_date { get; set; }

        public DateTime? arrival_date { get; set; }

        public int? transfers_count { get; set; }

        public decimal? price { get; set; }

        public string? currency { get; set; }

        public string? flight_number { get; set; }
    }

}
namespace FlightAPI.Common
{
    public class AppSettings
    {
        public string? DataSources { get; set; }
        public int Threshold { get; set; }
        public int CacheExpirationTimeInSec { get; set; }
    }
}


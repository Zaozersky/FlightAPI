namespace FlightAPI.Common
{
	public class AggregatedFlightServiceNotFoundException : Exception
	{
        public AggregatedFlightServiceNotFoundException()
        {
        }

        public AggregatedFlightServiceNotFoundException(string message)
            : base(message)
        {
        }

        public AggregatedFlightServiceNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}


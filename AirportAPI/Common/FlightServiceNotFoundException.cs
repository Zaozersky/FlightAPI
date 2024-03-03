namespace AirportAPI.Common
{
	public class FlightServiceNotFoundException : Exception
	{
        public FlightServiceNotFoundException()
        {
        }

        public FlightServiceNotFoundException(string message)
            : base(message)
        {
        }

        public FlightServiceNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}


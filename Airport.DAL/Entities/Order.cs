namespace Airport.DAL.Entities;

public class Order
{
    public int Id { get; set; }

    public int? FlightId { get; set; }

    public virtual Flight? Flight { get; set; }
}

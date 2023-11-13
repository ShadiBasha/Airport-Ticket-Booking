namespace AirportTicketBooking;

public class User
{
    public string Name { get; private set; }
    private int Password { get; set; }
    private List<FlightDetails> _bookings;

    public User(string name, int password)
    {
        Name = name;
        Password = password;
        _bookings = new List<FlightDetails>();
    }

    public void BookAFlight(FlightDetails flightDetails)
    {
        _bookings.Add(flightDetails);
    }

    public List<FlightDetails> GetBookings()
    {
        return _bookings;
    }
}
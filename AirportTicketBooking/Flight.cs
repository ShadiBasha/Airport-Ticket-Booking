namespace AirportTicketBooking;

public class Flight
{
    private static int _id;
    public int Id { get; init; }
    private List<Trip> _trips;
    public Flight(List<Trip> trips)
    {
        Id = _id;
        _trips = trips;
        _id++;
    }

    public bool addTrip(Trip trip)
    {
        _trips.Add(trip);
        return true;
    }
}
namespace AirportTicketBooking;

public class Flight
{
    private static int _id;
    public int Id { get; init; }
    private List<TripDetails> _trips;
    public Flight(List<TripDetails> trips)
    {
        Id = _id;
        _trips = trips;
        _id++;
    }

    public bool addTrip(TripDetails tripDetails)
    {
        _trips.Add(tripDetails);
        return true;
    }
}
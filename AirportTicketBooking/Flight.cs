namespace AirportTicketBooking;

public class Flight
{
    private static int _id;
    public int Id { get; init; }
    // private List<TripDetails> _trips;

    public int FlightPlanId { get; set; }
    public TimeOnly Duration { get; set; }
    private DateTime _takeoffTime;
    public DateTime TakeoffTime
    {
        get => _takeoffTime;
        set
        {
            if (value > DateTime.UtcNow)
            {
                _takeoffTime = value;
            }
            else
            {
                _takeoffTime = DateTime.MaxValue;
            }
        }
    }
    
    public Airport DepartureAirportId { get; set;}

    public Airport ArrivalAirportId { get; set; }

    public Flight(DateTime takeoffTime, int flightPlanId, TimeOnly duration, Airport departureAirportId, Airport arrivalAirportId)
    {
        _takeoffTime = takeoffTime;
        Id = _id;
        FlightPlanId = flightPlanId;
        Duration = duration;
        DepartureAirportId = departureAirportId;
        ArrivalAirportId = arrivalAirportId;
        _id++;
    }

    // public bool addTrip(TripDetails tripDetails)
    // {
    //     _trips.Add(tripDetails);
    //     return true;
    // }
}

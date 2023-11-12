namespace AirportTicketBooking;

public class FlightDetails : Flight
{
    public int Id { get; init; }
    // private List<TripDetails> _trips;
    public int PlanId { get; set; }
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
    public int DepartureAirportId { get; set;}
    public int ArrivalAirportId { get; set; }
    
    public FlightDetails(DateTime takeoffTime, int planId, TimeOnly duration, int departureAirportId, int arrivalAirportId)
    {
        _takeoffTime = takeoffTime;
        Id = IdGenerator;
        PlanId = planId;
        Duration = duration;
        DepartureAirportId = departureAirportId;
        ArrivalAirportId = arrivalAirportId;
        IdGenerator++;
    }

    public override string ToString()
    {
        return $"""
                Flight {Id}
                Plan Id : {PlanId}
                Departure Airport : {DepartureAirportId} --> Arrival Airport : {ArrivalAirportId}
                Takeoff Date : {TakeoffTime}
                Duration : {Duration.Hour}:{Duration.Minute.ToString().PadLeft(2,'0')}
                """;
    }
    // public bool addTrip(TripDetails tripDetails)
    // {
    //     _trips.Add(tripDetails);
    //     return true;
    // }
}

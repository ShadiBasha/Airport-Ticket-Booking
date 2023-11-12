using System.Diagnostics.CodeAnalysis;

namespace AirportTicketBooking;

public class TripDetails : Trip
{
    public int Id { get; init; }
    private DateTime _takeoffTime;
    public required TimeOnly Duration { get; set; }
    public required PlanDetails TripPlanDetails { get; set; }
    public required DateTime TakeoffTime
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

    // public required Airport Departure { get; set;}

    // public required Airport Arrival { get; set; }

    [SetsRequiredMembers]
    public TripDetails(DateTime takeoffTime, TimeOnly duration, PlanDetails tripPlanDetails, Airport departure, Airport arrival)
    {
        Id = IdGenerator;
        TakeoffTime = takeoffTime;
        Duration = duration;
        TripPlanDetails = tripPlanDetails;
        // Departure = departure;
        // Arrival = arrival;
        IdGenerator++;
    }
    
}
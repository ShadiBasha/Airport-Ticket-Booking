﻿namespace AirportTicketBooking;

public class Trip
{
    private static int _id; 
    public int Id { get; init; }
    private DateTime _takeoffTime;
    public required TimeOnly Duration { get; set; }
    public required Plan TripPlan { get; set; }
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

    public Trip(DateTime takeoffTime, TimeOnly duration, Plan tripPlan)
    {
        Id = _id;
        TakeoffTime = takeoffTime;
        Duration = duration;
        TripPlan = tripPlan;
        _id++;
    }


}
﻿using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Details;

public class FlightDetails : IIndexed
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
    
    public FlightDetails(int id,DateTime takeoffTime, int planId, TimeOnly duration, int departureAirportId, int arrivalAirportId)
    {
        Id = id;
        _takeoffTime = takeoffTime;
        PlanId = planId;
        Duration = duration;
        DepartureAirportId = departureAirportId;
        ArrivalAirportId = arrivalAirportId;
    }

    public int[] ComputePrices()
    {
        PlanStorage planStorage = PlanStorage.GetStorageInstance();
        planStorage.ReadFile();
        var plan = planStorage.FindPlan(PlanId);
        int economyPrice = plan.EconomyPrice * Duration.Hour * 2 + plan.EconomyPrice * (Duration.Minute / 30);
        int businessPrice = plan.BusinessPrice * Duration.Hour * 2 + plan.BusinessPrice * (Duration.Minute / 30);
        int firstClassPrice = plan.FirstClassPrice * Duration.Hour * 2 + plan.FirstClassPrice * (Duration.Minute / 30);
        return new [] {economyPrice,businessPrice,firstClassPrice};
    }

    public override string ToString()
    {
        AirportStorage airportStorage = AirportStorage.GetStorageInstance();
        var airportDetails = airportStorage.GetData();
        var prices = ComputePrices();
        return $"""
                Flight {Id}
                Plan Id : {PlanId}
                Departure Airport : {airportDetails[DepartureAirportId].Name} --> Arrival Airport : {airportDetails[ArrivalAirportId].Name}
                Takeoff Date : {TakeoffTime}
                Duration : {Duration.Hour}:{Duration.Minute.ToString().PadLeft(2,'0')}
                Price
                Economy     : {prices[0]}$ 
                Business    : {prices[1]}$
                First Class : {prices[2]}$
                """;
    }
    // public bool addTrip(TripDetails tripDetails)
    // {
    //     _trips.Add(tripDetails);
    //     return true;
    // }
}

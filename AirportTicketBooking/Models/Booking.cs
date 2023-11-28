using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Models;

public class Booking : IIndexed
{
    public int Id { get; init; }
    public int FlightId { get; init; }
    public int UserId { get; init; }
    public Classes ClassType { get; init; }
    public Booking(int id, int flightId, int userId, Classes classType)
    {
        Id = id;
        FlightId = flightId;
        UserId = userId;
        ClassType = classType;
    }

    public override string ToString()
    {
        var userStorage = StorageFactory.GetStorage(StorageType.User) as UserStorage;
        var flightStorage = StorageFactory.GetStorage(StorageType.Flight) as FlightStorage;
        return $"""
               ****************************
               Booked flight for user {UserId} 
               Username  : {userStorage?.FindUser(UserId)?.Name}
               User Class: {ClassType}
               Booking ID : {Id}
               {flightStorage?.FindFlight(FlightId)}
               ****************************
               """;
    }
}
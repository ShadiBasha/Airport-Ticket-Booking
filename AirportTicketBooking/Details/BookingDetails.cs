using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Details;

public class BookingDetails : IIndexed
{
    public int Id { get; init; }
    public int FlightId { get; init; }
    public int UserId { get; init; }
    public Classes ClassType { get; init; }
    public BookingDetails(int id, int flightId, int userId, Classes classType)
    {
        Id = id;
        FlightId = flightId;
        UserId = userId;
        ClassType = classType;
    }

    public override string ToString()
    {
        UserStorage userStorage = UserStorage.GetStorageInstance();
        FlightStorage flightStorage = FlightStorage.GetStorageInstance();
        return $"""
               ****************************
               Booked flight for user {UserId} 
               Username  : {userStorage.FindUser(UserId)?.Name}
               User Class: {ClassType}
               {flightStorage.FindFlight(FlightId)}
               ****************************
               """;
    }
}
using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Security;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Details;

public class UserDetails : IIndexed
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public int Password { get; private set; }
    public byte[] Salt { get; private set; }
    public List<int> BookingIds { get; private set; }
    public UserDetails(int id, string name, string password)
    {
        Id = id;
        Name = name;
        Salt = HashGenerator.GenerateSalt();
        Password = HashGenerator.HashPassword(password, Salt);        
        BookingIds = new List<int>();
    }
    public void BookAFlight(int flightId, Classes classType)
    {
        FlightStorage flightStorage = FlightStorage.GetStorageInstance();
        if (flightStorage.FindFlight(flightId) == null)
        {
            throw new Exception("Error : Flight does not Exists");
        }
        BookingStorage bookingStorage = BookingStorage.GetStorageInstance();
        var bookingId = bookingStorage.GetCurrentId();
        bookingStorage.AddData(new BookingDetails(bookingId, flightId, Id, classType));
        BookingIds.Add(bookingId);
    }

    public void ModifyBooking(int bookingId, Classes newClass)
    {
        BookingStorage bookingStorage = BookingStorage.GetStorageInstance();
        if (bookingStorage.FindBooking(bookingId) != null)
        {
            var flightId = bookingStorage.FindBooking(bookingId).FlightId;
            bookingStorage.DeleteData(bookingId);
            bookingStorage.AddData(new BookingDetails(bookingId, flightId, Id, newClass));
        }
        else
        {
            throw new Exception("Error: Your didn't book this flight");
        }
    }
    
    public void CancelBooking(int bookingId)
    {
        BookingStorage bookingStorage = BookingStorage.GetStorageInstance();
        if (bookingStorage.FindBooking(bookingId) != null)
        {
            bookingStorage.DeleteData(bookingId);
            BookingIds.Remove(bookingId);
        }
        else
        {
            throw new Exception("Error: Can't Cancel a booking that your not registered to");
        }
    }

    public override string ToString()
    {
        return $"""
                User {Id}
                Username : {Name}
                """;
    }
}
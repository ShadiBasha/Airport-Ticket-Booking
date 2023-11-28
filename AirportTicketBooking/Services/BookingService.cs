using AirportTicketBooking.Enum;
using AirportTicketBooking.Models;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Services;

public static class BookingService
{
    public static void BookAFlight(int flightId, Classes classType, User? user)
    {
        if (user is null)
            throw new Exception("Error: The User Can not be Null");
        
        var flightStorage = StorageFactory.GetStorage(StorageType.Flight) as FlightStorage;
        if (flightStorage?.FindFlight(flightId) == null)
        {
            throw new Exception("Error : Flight does not Exists");
        }
        var bookingStorage = StorageFactory.GetStorage(StorageType.Booking) as BookingStorage;
        var bookingId = bookingStorage?.GetCurrentId();
        bookingStorage?.AddData(new Booking((int)bookingId!, flightId, user.Id, classType));
        user.BookingIds.Add((int)bookingId!);
    }

    public static void ModifyBooking(int bookingId, Classes newClass, User? user)
    {
        if (user is null)
            throw new Exception("Error: The User Can not be Null");
        
        var bookingStorage = StorageFactory.GetStorage(StorageType.Booking) as BookingStorage;
        if (bookingStorage?.FindBooking(bookingId) != null && user.BookingIds.Contains(bookingId))
        {
            var flightId = bookingStorage?.FindBooking(bookingId)?.FlightId;
            bookingStorage?.DeleteData(bookingId);
            bookingStorage?.AddData(new Booking(bookingId, (int)flightId!, user.Id, newClass));
        }
        else
        {
            throw new Exception("Error: Your didn't book this flight");
        }
    }
    
    public static void CancelBooking(int bookingId, User? user)
    {
        if (user is null)
            throw new Exception("Error: The User Can not be Null");
        
        var bookingStorage = StorageFactory.GetStorage(StorageType.Booking) as BookingStorage;
        if (bookingStorage?.FindBooking(bookingId) != null && user.BookingIds.Contains(bookingId))
        {
            bookingStorage.DeleteData(bookingId);
            user.BookingIds.Remove(bookingId);
        }
        else
        {
            throw new Exception("Error: Can't Cancel a booking that your not registered to");
        }
    }
}
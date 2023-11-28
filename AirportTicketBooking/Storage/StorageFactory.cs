using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;

namespace AirportTicketBooking.Storage;

public class StorageFactory
{
    private static AirportStorage? _airportStorage;
    private static PlanStorage? _planStorage;
    private static FlightStorage? _flightStorage;
    private static BookingStorage? _bookingStorage;
    private static UserStorage? _userStorage;
    public static IStorage? GetStorage(StorageType storageType, string? path = null)
    {
        switch (storageType)
        {
            case StorageType.Airport when _airportStorage == null:
                _airportStorage = new AirportStorage(path);
                return _airportStorage;
            case StorageType.Airport:
                return _airportStorage;
            case StorageType.Plan when _planStorage == null:
                _planStorage = new PlanStorage(path);
                return _planStorage;
            case StorageType.Plan:
                return _planStorage;
            case StorageType.Flight when _flightStorage == null:
                _flightStorage = new FlightStorage(path);
                return _flightStorage;
            case StorageType.Flight:
                return _flightStorage;
            case StorageType.Booking when _bookingStorage == null:
                _bookingStorage = new BookingStorage(path);
                return _bookingStorage;
            case StorageType.Booking:
                return _bookingStorage;
            case StorageType.User when _userStorage == null:
                _userStorage = new UserStorage(path);
                return _userStorage;
            case StorageType.User:
                return _userStorage;
        }

        return null;
    } 
}
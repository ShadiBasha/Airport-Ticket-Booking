using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Models;
using ServiceStack;

namespace AirportTicketBooking.Storage;

public class BookingStorage : Storage<Booking>, IStorage
{
    public BookingStorage(string path) : base(path) {}
    public Booking? FindBooking(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var booking) ? booking : null;
    }
    ~BookingStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }

    }
    
    public Dictionary<int, Booking> GetAFlightBookings(int flightId)
    {
        return _dataDetailsMap
            .Values
            .Where(booking => booking.FlightId == flightId)
            .ToDictionary(data => data.Id);
    }
    
    public Dictionary<int, Booking> GetBookingsInPriceRange(FlightStorage flightStorage, int minPrice, int maxPrice)
    {
        var flightsInRange = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.ComputePrices()[0] < maxPrice && flight.ComputePrices()[0] > minPrice)
            .Select(flight => Tuple.Create(flight.Id,flight.ComputePrices()))
            .ToList();
        return _dataDetailsMap
            .Values
            .Join(flightsInRange, booking => booking.FlightId, flight => flight.Item1,
                (booking, flight) => Tuple.Create(booking, flight.Item2))
            .Where(booking => booking.Item2[(int)booking.Item1.ClassType] >= minPrice &&
                               booking.Item2[(int)booking.Item1.ClassType] <= maxPrice)
            .ToDictionary(data => data.Item1.Id, data => data.Item1);
    }
    
    public Dictionary<int, Booking> GetBookingsFromCountry(FlightStorage flightStorage, AirportStorage airportStorage,
        Country departureCountry)
    {
        var airportsInCountry = airportStorage
            .GetData()
            .Values
            .Where(airport => airport.AirportCountry == departureCountry)
            .Select(airport => airport.Id)
            .ToList();
        var flightFromCountry = flightStorage
            .GetData()
            .Values
            .Join(airportsInCountry, flight => flight.DepartureAirportId, airport => airport,
                (flight, airport) => flight.Id)
            .ToList();
        return _dataDetailsMap
            .Values
            .Join(flightFromCountry, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public Dictionary<int, Booking> GetBookingsToCountry(FlightStorage flightStorage, AirportStorage airportStorage,
        Country arrivalCountry)
    {
        var airportsInCountry = airportStorage
            .GetData()
            .Values
            .Where(airport => airport.AirportCountry == arrivalCountry)
            .Select(airport => airport.Id)
            .ToList();
        var flightToCountry = flightStorage
            .GetData()
            .Values
            .Join(airportsInCountry, flight => flight.ArrivalAirportId, airport => airport,
                (flight, airport) => flight.Id)
            .ToList();
        return _dataDetailsMap
            .Values
            .Join(flightToCountry, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public Dictionary<int, Booking> GetBookingsFromAirport(FlightStorage flightStorage, int fromAirportId)
    {
        var flightFromAirport = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.DepartureAirportId == fromAirportId)
            .Select(flight => flight.Id)
            .ToList();
        return _dataDetailsMap
            .Values
            .Join(flightFromAirport, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public Dictionary<int, Booking> GetBookingsToAirport(FlightStorage flightStorage, int toAirportId)
    {
        var flightToAirport = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.ArrivalAirportId == toAirportId)
            .Select(flight => flight.Id)
            .ToList();
        return _dataDetailsMap
            .Values
            .Join(flightToAirport, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public Dictionary<int, Booking> GetUserBookings(int userId)
    {
        return _dataDetailsMap
            .Values
            .Where(booking => booking.UserId == userId)
            .ToDictionary(x => x.Id);
    }
    
    public Dictionary<int, Booking> GetBookingsWithClass(Classes classType)
    {
        return _dataDetailsMap
            .Values
            .Where(booking => booking.ClassType == classType)
            .ToDictionary(x => x.Id);
    }

    public IEnumerable<Booking> FilterByAll(FlightStorage flightStorage, AirportStorage airportStorage,int? flightId, int? minPrice, int? maxPrice,Country? departureCountry,Country? arrivalCountry,int? fromAirportId,int? toAirportId,int? userId, Classes? classType)
    {
        var bookings = _dataDetailsMap.CreateCopy();
        bookings = flightId != null ? GetAFlightBookings((int)flightId) : bookings;
        bookings = minPrice != null && maxPrice != null ? GetBookingsInPriceRange(flightStorage, (int)minPrice,(int)maxPrice) : bookings;
        bookings = departureCountry != null ? GetBookingsFromCountry(flightStorage,airportStorage, (Country)departureCountry) : bookings;
        bookings = arrivalCountry != null ? GetBookingsToCountry(flightStorage,airportStorage, (Country)arrivalCountry) : bookings;
        bookings = fromAirportId != null ? GetBookingsFromAirport(flightStorage, (int)fromAirportId) : bookings;
        bookings = toAirportId != null ? GetBookingsToAirport(flightStorage, (int)toAirportId) : bookings;
        bookings = userId != null ? GetUserBookings((int)userId) : bookings;
        bookings = classType != null ? GetBookingsWithClass((Classes)classType) : bookings;
        foreach (var booking in bookings.Values)
        {
            yield return booking;
        }
    }
}
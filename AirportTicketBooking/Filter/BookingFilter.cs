using AirportTicketBooking.Details;
using AirportTicketBooking.Enum;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Filter;

public static class BookingFilter
{
    public static Dictionary<int, BookingDetails> GetAFlightBookings(Dictionary<int, BookingDetails> bookings, int flightId)
    {
        return bookings
            .Values
            .Where(booking => booking.FlightId == flightId)
            .ToDictionary(data => data.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsInPriceRange(Dictionary<int, BookingDetails> bookings, FlightStorage flightStorage, int minPrice, int maxPrice)
    {
        var flightsInRange = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.ComputePrices()[0] < maxPrice && flight.ComputePrices()[0] > minPrice)
            .Select(flight => Tuple.Create(flight.Id,flight.ComputePrices()))
            .ToList();
        return bookings
            .Values
            .Join(flightsInRange, booking => booking.FlightId, flight => flight.Item1,
                (booking, flight) => Tuple.Create(booking, flight.Item2))
            .Where(booking => booking.Item2[(int)booking.Item1.ClassType] >= minPrice &&
                               booking.Item2[(int)booking.Item1.ClassType] <= maxPrice)
            .ToDictionary(data => data.Item1.Id, data => data.Item1);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsFromCountry(Dictionary<int, BookingDetails> bookings, FlightStorage flightStorage, AirportStorage airportStorage,
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
        return bookings
            .Values
            .Join(flightFromCountry, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsToCountry(Dictionary<int, BookingDetails> bookings, FlightStorage flightStorage, AirportStorage airportStorage,
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
        return bookings
            .Values
            .Join(flightToCountry, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsFromAirport(Dictionary<int, BookingDetails> bookings, FlightStorage flightStorage, int fromAirportId)
    {
        var flightFromAirport = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.DepartureAirportId == fromAirportId)
            .Select(flight => flight.Id)
            .ToList();
        return bookings
            .Values
            .Join(flightFromAirport, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsToAirport(Dictionary<int, BookingDetails> bookings, FlightStorage flightStorage, int toAirportId)
    {
        var flightToAirport = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.ArrivalAirportId == toAirportId)
            .Select(flight => flight.Id)
            .ToList();
        return bookings
            .Values
            .Join(flightToAirport, booking => booking.FlightId, flight => flight, (booking, flight) => booking)
            .ToDictionary(x => x.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetUserBookings(Dictionary<int, BookingDetails> bookings, int userId)
    {
        return bookings
            .Values
            .Where(booking => booking.UserId == userId)
            .ToDictionary(x => x.Id);
    }
    
    public static Dictionary<int, BookingDetails> GetBookingsWithClass(Dictionary<int, BookingDetails> bookings, Classes classType)
    {
        return bookings
            .Values
            .Where(booking => booking.ClassType == classType)
            .ToDictionary(x => x.Id);
    }

    public static IEnumerable<BookingDetails> FilterByAll(Dictionary<int, BookingDetails> bookings,FlightStorage flightStorage, AirportStorage airportStorage,int? flightId, int? minPrice, int? maxPrice,Country? departureCountry,Country? arrivalCountry,int? fromAirportId,int? toAirportId,int? userId, Classes? classType)
    {
        bookings = flightId != null ? GetAFlightBookings(bookings, (int)flightId) : bookings;
        bookings = minPrice != null && maxPrice != null ? GetBookingsInPriceRange(bookings,flightStorage, (int)minPrice,(int)maxPrice) : bookings;
        bookings = departureCountry != null ? GetBookingsFromCountry(bookings,flightStorage,airportStorage, (Country)departureCountry) : bookings;
        bookings = arrivalCountry != null ? GetBookingsToCountry(bookings,flightStorage,airportStorage, (Country)arrivalCountry) : bookings;
        bookings = fromAirportId != null ? GetBookingsFromAirport(bookings,flightStorage, (int)fromAirportId) : bookings;
        bookings = toAirportId != null ? GetBookingsToAirport(bookings,flightStorage, (int)toAirportId) : bookings;
        bookings = userId != null ? GetUserBookings(bookings, (int)userId) : bookings;
        bookings = classType != null ? GetBookingsWithClass(bookings, (Classes)classType) : bookings;
        foreach (var booking in bookings.Values)
        {
            yield return booking;
        }
    }
}
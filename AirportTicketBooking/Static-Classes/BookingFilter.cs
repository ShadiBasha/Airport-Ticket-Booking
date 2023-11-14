using AirportTicketBooking.Enum;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Static_Classes;

public static class BookingFilter
{
    static BookingFilter()
    {
    }

    public static List<Tuple<Classes? , int>> GetAFlightBookings(UserStorage userStorage, int flightId)
    {
        return userStorage
            .GetData()
            .Values
            .Select(userData => userData.Bookings)
            .Select(booking => booking.FirstOrDefault(book => book.Item1 == flightId))
            .GroupBy(book => book?.Item2)
            .Select(group => Tuple.Create(group.Key, group.Count())).ToList();
    }

    public static List<Tuple<int,Classes, int,int>> GetBookingInPriceRange(UserStorage userStorage,FlightStorage flightStorage, int minPrice, int maxPrice)
    {
        var flightsInRange = flightStorage
            .GetData()
            .Values
            .Where(flight => flight.ComputePrices()[0] < maxPrice && flight.ComputePrices()[0] > minPrice)
            .Select(flight => Tuple.Create(flight.Id,flight.ComputePrices()))
            .ToList();
        var bookings = userStorage
            .GetData()
            .Values
            .SelectMany(user => user.Bookings)
            .ToList();
        return flightsInRange
            .Join(bookings, flight => flight.Item1, booking => booking.Item1,
                (flight, booking) => Tuple.Create(flight.Item1, booking.Item2, flight.Item2[(int)booking.Item2]))
            .Where(booking => booking.Item3 >= minPrice && booking.Item3 <= maxPrice)
            .GroupBy(booking => new { Key1 = booking.Item1, Key2 = booking.Item2 })
            .Select(group => Tuple.Create(group.Key.Key1, group.Key.Key2,group.Count(),group.Sum(booking => booking.Item3)))
            .ToList();
    }
}
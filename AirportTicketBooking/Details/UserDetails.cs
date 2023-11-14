using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Static_Classes;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Details;

public class UserDetails : IIndexed
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public int Password { get; private set; }
    public byte[] Salt { get; private set; }
    public List<Tuple<int, Classes>> Bookings { get; private set; }
    public UserDetails(int id, string name, string password)
    {
        Id = id;
        Name = name;
        Salt = HashGenerator.GenerateSalt();
        Password = HashGenerator.HashPassword(password, Salt);        
        Bookings = new List<Tuple<int, Classes>>();
    }
    public void BookAFlight(int flightId, Classes classType)
    {
        FlightStorage flightStorage = FlightStorage.GetStorageInstance();
        if (flightStorage.FindFlight(flightId) == null)
        {
            throw new Exception("Error : Flight does not Exists");
        }

        Bookings.Add(Tuple.Create(flightId, classType));
    }

    public void ModifyBooking(int flightId, Classes newClass)
    {
        if (Bookings.Any(booking => booking.Item1 == flightId))
        {
            var newBookings = Bookings
                .Select(booking => booking.Item1 == flightId ? Tuple.Create(flightId, newClass) : booking).ToList();
            Bookings = newBookings;
        }
        else
        {
            throw new Exception("Error: Your didn't book this flight");
        }
    }
    
    public void CancelBooking(int flightId)
    {
        if (Bookings.Any(booking => booking.Item1 == flightId))
        {
            var newBookings = Bookings.Where(booking => booking.Item1 != flightId).ToList();
            Bookings = newBookings;
        }
        else
        {
            throw new Exception("Error: Can't Cancel a booking that your not registered to");
        }
    }

    public override string ToString()
    {
        FlightStorage flightStorage = FlightStorage.GetStorageInstance();
        string data = "\n****************************\n";
        foreach (var book in Bookings)
        {
            data += $"""
                    User class : {book.Item2}
                    {flightStorage.FindFlight(book.Item1)}
                    ****************************
                    
                    """;
        }
        return $"""
                User {Id}
                Username : {Name}
                Flights
                ****************************
                {data}
                """;
    }
}
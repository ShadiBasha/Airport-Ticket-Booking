namespace AirportTicketBooking;

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
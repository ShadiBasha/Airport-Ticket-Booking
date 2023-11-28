using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Security;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking.Models;

public class User : IIndexed
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public int Password { get; private set; }
    public byte[] Salt { get; private set; }
    public List<int> BookingIds { get; private set; }
    public User(int id, string name, string password)
    {
        Id = id;
        Name = name;
        Salt = HashGenerator.GenerateSalt();
        Password = HashGenerator.HashPassword(password, Salt);        
        BookingIds = new List<int>();
    }

    public override string ToString()
    {
        return $"""
                User {Id}
                Username : {Name}
                """;
    }
}
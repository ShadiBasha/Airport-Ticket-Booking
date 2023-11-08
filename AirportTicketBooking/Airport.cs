using System.Reflection;

namespace AirportTicketBooking;

public class Airport
{
    private static int _id;
    public int Id { get; init; }
    public string Name { get; set; }
    public Country AirportCountry { get; set; }
    public string Address { get; set; }
    public Airport(string name, Country airportCountry, string address)
    {
        Id = _id;
        Name = name;
        AirportCountry = airportCountry;
        Address = address;
        _id++;
    }
}
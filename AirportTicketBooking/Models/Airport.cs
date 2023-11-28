using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;

namespace AirportTicketBooking.Models;

public class Airport : IIndexed
{
    public int Id { get; init; }
    public string Name { get; set; }
    public Country AirportCountry { get; set; }
    public string Address { get; set; }
    public Airport(int id,string name, Country airportCountry, string address)
    {
        Id = id;
        Name = name;
        AirportCountry = airportCountry;
        Address = address;
    }

    public override string ToString()
    {
        return $"""
                Airport {Id}
                Airport Name    : {Name}
                Airport Country : {AirportCountry}
                Address         : {Address}
                """;
    }
}



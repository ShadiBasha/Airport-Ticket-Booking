using System.Reflection;

namespace AirportTicketBooking;

public class AirportDetails : Airport
{
    public int Id { get; init; }
    public string Name { get; set; }
    public Country AirportCountry { get; set; }
    public string Address { get; set; }
    public AirportDetails(string name, Country airportCountry, string address)
    {
        Id = IdGenerator;
        Name = name;
        AirportCountry = airportCountry;
        Address = address;
        IdGenerator++;
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



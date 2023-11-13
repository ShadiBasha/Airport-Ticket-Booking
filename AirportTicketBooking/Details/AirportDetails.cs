using System.Reflection;

namespace AirportTicketBooking;

public class AirportDetails : IIndexed
{
    public int Id { get; init; }
    public string Name { get; set; }
    public Country AirportCountry { get; set; }
    public string Address { get; set; }
    public AirportDetails(int id,string name, Country airportCountry, string address)
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



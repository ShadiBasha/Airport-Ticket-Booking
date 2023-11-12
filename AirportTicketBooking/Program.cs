

using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        FlightStorage f = FlightStorage.GetStorageInstance();
        f.ReadFile();
        Console.WriteLine(f);
        Dictionary<int, FlightDetails> temp = new();
        temp = FlightFilter.FilterByArrivalAirport(f.GetFlights(),Airport.FrankfurtAirport);
        temp = FlightFilter.FilterById(temp,0);
        Console.WriteLine();
        foreach (var VARIABLE in temp)
        {
            Console.WriteLine(VARIABLE.Value);
        }
        return 0;
    }
}


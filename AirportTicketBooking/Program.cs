using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        FlightStorage fs = FlightStorage.GetStorageInstance();
        fs.ReadFile();
        Console.WriteLine(fs);
        return 0;
    }
}


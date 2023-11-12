using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        var ais = AirportStorage.GetStorageInstance();
        ais.ReadFile();
        Console.WriteLine(ais);
        return 0;
    }
}


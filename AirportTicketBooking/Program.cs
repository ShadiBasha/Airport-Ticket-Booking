

using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        FlightStorage f = FlightStorage.GetStorageInstance();
        f.WriteFromAUserFile(@"C:\Users\Shadi Basha\Desktop\Shadi.csv");
        f.ReadFile();
        Console.WriteLine(f);
        return 0;
    }
}


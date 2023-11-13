using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        PlanStorage ps = PlanStorage.GetStorageInstance();
        ps.WriteInFile();
        Console.WriteLine(ps);
        return 0;
    }
}


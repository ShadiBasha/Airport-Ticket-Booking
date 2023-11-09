

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        PlanStorage ps = PlanStorage.GetStorageInstanse();
        ps.ReadFile();
        Console.WriteLine(ps.ToString());
        return 0;
    }
}


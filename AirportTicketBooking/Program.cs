

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        PlanStorage ps = PlanStorage.GetStorageInstanse();
        ps.ReadFile();
        Console.WriteLine(ps.ToString());
        ps.AddPlan(new PlanDetails("dad", 10, 30));
        ps.WriteInFile();
        return 0;
    }
}


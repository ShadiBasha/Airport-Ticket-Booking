

using ServiceStack;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        // PlanStorage ps = PlanStorage.GetStorageInstanse();
        // ps.ReadFile();
        PlanDetails p = new PlanDetails("shadi");
        Airport a1 = new Airport("A1", Country.Palestine, "jenin");
        Airport a2 = new Airport("A2", Country.Brazil, "reo");
        TripDetails t1 = new TripDetails(DateTime.Now, TimeOnly.Parse("1:00:0"), p, a1, a2);
        var lt = new List<TripDetails>();
        lt.Add(t1);
        Flight f = new Flight(lt);
        Console.WriteLine(f.ToCsv());
        return 0;
    }
}


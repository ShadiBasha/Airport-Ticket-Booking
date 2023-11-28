using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Models;

namespace AirportTicketBooking.Storage;

public class PlanStorage : Storage<Plan>, IStorage
{
    public PlanStorage(string path) : base (path) {}
    public Plan? FindPlan(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var plan) ? plan : null;
    }
    ~PlanStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
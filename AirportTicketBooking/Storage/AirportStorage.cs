using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Models;

namespace AirportTicketBooking.Storage;

public class AirportStorage : Storage<Airport>, IStorage
{
    public AirportStorage(string path) : base(path) {}
        
    public Airport? FindAirport(int? id)
    {
        if(id != null)
            return _dataDetailsMap.TryGetValue((int)id, out var airport) ? airport : null;
        return null;
    }

    ~AirportStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
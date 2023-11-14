using AirportTicketBooking.Details;
using ServiceStack;

namespace AirportTicketBooking.Storage;

public class FlightStorage : Storage<FlightDetails>
{
    public int IdGenerator { get; set; }
    private static FlightStorage? _flightStorage = null;

    public int GetCurrentId()
    {
        return IdGenerator++;
    }
    private FlightStorage(string? path)
    {
        _defaultPath = "FlightData.csv";
        _path = path ?? _defaultPath;
        IdGenerator = 0;    
        _dataDetailsMap = new Dictionary<int, FlightDetails>();
    }
    
    public FlightDetails? FindFlight(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var flight) ? flight : null;
    }
    
    public static FlightStorage GetStorageInstance(string? path = null)
    {
        if (_flightStorage == null)
        {
            _flightStorage = new FlightStorage(path);
        }
        _flightStorage.ReadFile();
        return _flightStorage;
    }

    public void WriteFromAUserFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception("Error: Can't access the file Check the file path and try again");
        }
        FlightStorage flightStorage = GetStorageInstance();
        PlanStorage planStorage = PlanStorage.GetStorageInstance();
        AirportStorage airportStorage = AirportStorage.GetStorageInstance();
        var newFlights = new List<FlightDetails>();
        var reader = File.OpenRead(path);
        var lines = reader.ReadLines();

        foreach (var line in lines)
        {
            int index = 0;
            var fields = line.Split(',');
            if (fields.Length > 5)
            {
                throw new Exception("Error: Extra data is provided please check the manual and try again");
            }
            
            int planId;
            int departureAirportId;
            int arrivalAirportId;
            DateTime takeoffTime;
            TimeOnly duration;

            try
            {
                planId = int.Parse(fields[0]);
                if (planStorage.FindPlan(planId) == null)
                {
                    throw new Exception("Error: plan does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the plan id field");
            }

            try
            {
                departureAirportId = int.Parse(fields[1]);
                if (airportStorage.FindAirport(departureAirportId) == null)
                {
                    throw new Exception("Error: Airport does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the departure airport field");
            }

            try
            {
                arrivalAirportId = int.Parse(fields[2]);
                if (airportStorage.FindAirport(arrivalAirportId) == null)
                {
                    throw new Exception("Error: Airport does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the arrival airport field");
            }
            
            try
            {
                takeoffTime = DateTime.Parse(fields[3]);
                if (takeoffTime < DateTime.Today)
                {
                    throw new Exception($"Error in Line {index}: The data provided is in the past change the data and try again");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is Data and time in the takeoff time field");
            }
            
            try
            {
                duration = TimeOnly.Parse(fields[4]);
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Line {index}: Make sure the value is a positive time in the duration field");
            }
            FlightDetails newFlightDetails =
                new FlightDetails(GetCurrentId(),takeoffTime, planId, duration, departureAirportId, arrivalAirportId);
            newFlights.Add(newFlightDetails);
        }
        
        foreach (var flight in newFlights)
        {
            IdGenerator++;
            _dataDetailsMap.Add(IdGenerator, flight);
        }
        flightStorage.WriteInFile();
    }

    ~FlightStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }

    protected override void SetGenerator(List<FlightDetails> detailsList)
    {
        try
        {
            IdGenerator = detailsList[^1].Id + 1;
        }
        catch (Exception e)
        {
            IdGenerator = 0;
        }
    }
}
using ServiceStack;

namespace AirportTicketBooking;

public class FlightStorage : Flight
{
    private static FlightStorage? _flightStorage = null;
    private string _path;
    public Dictionary<int,FlightDetails> _flightsDetailsMap;
    public bool SaveDataBeforeClosing {
        get;
        set;
    }
    private FlightStorage(string path)
    {
        _path = path;
        _flightsDetailsMap = new Dictionary<int, FlightDetails>();
    }
    public bool Add(FlightDetails flightDetails)
    {
        _flightsDetailsMap.Add(flightDetails.Id,flightDetails);
        return true;
    }
    public static FlightStorage GetStorageInstance(string path = "FlightData.csv")
    {
        if (_flightStorage == null)
        {
            
            _flightStorage = new FlightStorage(path);
        }
        
        return _flightStorage;
    }
    public bool ReadFile()
    {
        if (File.Exists(_path))
        {
            _flightsDetailsMap.Clear();
            string data = File.ReadAllText(_path);
            List<FlightDetails> flightDetailsList = data.FromCsv<List<FlightDetails>>();
            IdGenerator = flightDetailsList[^1].Id + 1;
            _flightsDetailsMap = flightDetailsList.Select(flight => new { flight.Id, flight = flight }).ToDictionary(x => x.Id,x=> x.flight);
            return true;
        }
        return false;
    }
    public bool WriteInFile()
    {
        if (!File.Exists(_path))
        {
            _path = "FlightData.csv";
        }
        List<FlightDetails> flgithDetails = _flightsDetailsMap.Values.ToList();
        File.WriteAllText(_path,flgithDetails.ToCsv());
        return true;
    }

    public void WriteFromAUserFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception("Error: Can't access the file Check the file path and try again");
        }
        FlightStorage flightStorage = GetStorageInstance();
        flightStorage.ReadFile();
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
                //find plan if it exists.
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the plan id field");
            }

            try
            {
                departureAirportId = int.Parse(fields[1]);
                //find airport if it exists.
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the departure airport field");
            }

            try
            {
                arrivalAirportId = int.Parse(fields[2]);
                //find airport if it exists
            }
            catch (Exception e)
            {
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
                new FlightDetails(takeoffTime, planId, duration, (Airport)departureAirportId, (Airport)arrivalAirportId);
            newFlights.Add(newFlightDetails);
        }
        
        foreach (var flight in newFlights)
        {
            IdGenerator++;
            _flightsDetailsMap.Add(IdGenerator, flight);
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
    public override string ToString()
    {
        string data ="****************************";
        foreach (var flight in _flightsDetailsMap)
        {
            data += $"""

                     {flight.Value}
                     ****************************
                     """;
        }
        return data;
    }
}
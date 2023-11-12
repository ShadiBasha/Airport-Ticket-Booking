using ServiceStack;

namespace AirportTicketBooking;

public class AirportStorage : Airport, IFileWriter, IFileReader
{
    private static AirportStorage? _airportStorage = null;
    private string _path;
    private Dictionary<int,AirportDetails> _airportsDetailsMap;
    public bool SaveDataBeforeClosing {
        get;
        set;
    }

    private AirportStorage(string path)
    {
        _path = path;
        _airportsDetailsMap = new Dictionary<int, AirportDetails>();
    }

    public bool Add(AirportDetails airportDetails)
    {
        _airportsDetailsMap.Add(airportDetails.Id,airportDetails);
        return true;
    }

    public bool FindAirport(int id)
    {
        if (_airportsDetailsMap.ContainsKey(id))
        {
            return true;
        }
        return false;
    }

    public Dictionary<int, AirportDetails> getAirports()
    {
        return _airportsDetailsMap;
    }

    public static AirportStorage GetStorageInstance(string path = "AirportData.csv")
    {
        if (_airportStorage == null)
        {
            _airportStorage = new AirportStorage(path);
        }
        
        return _airportStorage;
    }
    
    public bool ReadFile()
    {
        if (File.Exists(_path))
        {
            string data = File.ReadAllText(_path);
            List<AirportDetails> airportDetailsList = data.FromCsv<List<AirportDetails>>();
            Airport.IdGenerator = airportDetailsList[^1].Id + 1;
            _airportsDetailsMap = airportDetailsList
                .Select(airport => new { airport.Id, airport })
                .ToDictionary(x => x.Id,x=> x.airport);
            return true;
        }
        return false;
    }
    
    public bool WriteInFile()
    {
        if (!File.Exists(_path))
        {
            _path = "AirportData.csv";
        }

        List<AirportDetails> airportsDetails = _airportsDetailsMap.Values.ToList();
        File.WriteAllText(_path,airportsDetails.ToCsv());
        return true;
    }

    ~AirportStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }

    public override string ToString()
    {
        string data ="****************************";
        foreach (var airportsDetails in _airportsDetailsMap)
        {
            data += $"""
                    
                    {airportsDetails.Value}
                    ****************************   
                    """;
        }
        return data;
    }
}
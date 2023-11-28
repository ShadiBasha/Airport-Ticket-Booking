using AirportTicketBooking.Interfaces;
using ServiceStack;

namespace AirportTicketBooking.Storage;

public abstract class Storage<T> : IFileReader, IFileWriter, IStorage where T : IIndexed
{
    protected string _path;
    protected Dictionary<int, T> _dataDetailsMap = new();
    protected int IdGenerator { get; set; }
    public bool SaveDataBeforeClosing {
        get;
        set;
    }
    protected Storage(string path)
    {
        _path = path;
    }
    public int GetCurrentId()
    {
        return IdGenerator++;
    }
    private void SetGenerator()
    {
        try
        {
            var detailsList = _dataDetailsMap.Values.ToList();
            IdGenerator = detailsList[^1].Id + 1;
        }
        catch (Exception e)
        {
            IdGenerator = 0;
        }    
    }
    public void AddData(T dataDetails)
    {
        _dataDetailsMap.Add(dataDetails.Id,dataDetails); 
    }
    public void DeleteData(int id)
    {
        _dataDetailsMap.Remove(id);
    }
    public Dictionary<int, T> GetData()
    {
        return _dataDetailsMap;
    }
    public void WriteInFile()
    {
        if (!File.Exists(_path))
        {
            throw new Exception("Error: Path does not exist");
        }
        var dataDetails = _dataDetailsMap.Values.ToList();
        File.WriteAllText(_path,dataDetails.ToCsv());
    }
    public void ReadFile()
    {
        if (File.Exists(_path))
        {
            _dataDetailsMap.Clear();
            var data = File.ReadAllText(_path);
            var detailsList = data.FromCsv<List<T>>();
            SetGenerator();
            _dataDetailsMap = detailsList.Select(flight => new { flight.Id, flight = flight }).ToDictionary(x => x.Id,x=> x.flight);
            return;
        }
        WriteInFile();
    }
    
    public override string ToString()
    {
        var data ="****************************";
        foreach (var details in _dataDetailsMap)
        {
            data += $"""

                     {details.Value}
                     ****************************
                     """;
        }
        return data;
    }   
}
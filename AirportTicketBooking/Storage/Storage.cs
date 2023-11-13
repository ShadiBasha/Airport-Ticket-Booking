using System.Data.Common;
using ServiceStack;

namespace AirportTicketBooking;

public abstract class Storage<T> : IFileReader, IFileWriter where T : IIndexed
{
    protected string _path;
    protected string _defaultPath;
    protected Dictionary<int,T> _dataDetailsMap;
    public bool SaveDataBeforeClosing {
        get;
        set;
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
            _path = _defaultPath;
        }
        List<T> dataDetails = _dataDetailsMap.Values.ToList();
        File.WriteAllText(_path,dataDetails.ToCsv());
    }
    protected abstract void SetGenerator(List<T> detailsList);
    public void ReadFile()
    {
        if (File.Exists(_path))
        {
            _dataDetailsMap.Clear();
            string data = File.ReadAllText(_path);
            List<T> detailsList = data.FromCsv<List<T>>();
            SetGenerator(detailsList);
            _dataDetailsMap = detailsList.Select(flight => new { flight.Id, flight = flight }).ToDictionary(x => x.Id,x=> x.flight);
            return;
        }
        WriteInFile();
    }
    
    public override string ToString()
    {
        string data ="****************************";
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
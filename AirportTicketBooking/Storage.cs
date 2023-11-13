using ServiceStack;

namespace AirportTicketBooking;

public abstract class Storage<T> where T : IIndexed
{
    protected Storage<T>? _storage;
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
    public void DeletData(int id)
    {
        _dataDetailsMap.Remove(id);
    }
    public Dictionary<int, T> GetData()
    {
        return _dataDetailsMap;
    }
    public abstract T GetStorageInstance(string path);
    public bool WriteInFile()
    {
        if (!File.Exists(_path))
        {
            _path = _defaultPath;
        }
        List<T> dataDetails = _dataDetailsMap.Values.ToList();
        File.WriteAllText(_path,dataDetails.ToCsv());
        return true;
    }
    protected abstract void SetGenerator(List<T> detailsList);
    public bool ReadFile()
    {
        if (File.Exists(_path))
        {
            _dataDetailsMap.Clear();
            string data = File.ReadAllText(_path);
            List<T> detailsList = data.FromCsv<List<T>>();
            SetGenerator(detailsList);
            _dataDetailsMap = detailsList.Select(flight => new { flight.Id, flight = flight }).ToDictionary(x => x.Id,x=> x.flight);
            return true;
        }
        return false;
    }
    public abstract override string ToString();
}
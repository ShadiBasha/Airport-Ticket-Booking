using AirportTicketBooking.Details;

namespace AirportTicketBooking.Storage;

public class PlanStorage : Storage<PlanDetails>
{
    private int IdGenerator { get; set; }
    private static PlanStorage? _planStorage = null;

    private PlanStorage(string? path)
    {
        _defaultPath = "PlanData.csv";
        _path = path ?? _defaultPath;
        IdGenerator = 0;
        _dataDetailsMap = new Dictionary<int, PlanDetails>();
    }

    public PlanDetails? FindPlan(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var plan) ? plan : null;
    }
    
    public int GetCurrentId()
    {
        return IdGenerator++;
    }

    public static PlanStorage GetStorageInstance(string? path = null)
    {
        if (_planStorage == null)
        {
            _planStorage = new PlanStorage(path);
        }
        return _planStorage;
    }

    protected override void SetGenerator(List<PlanDetails> detailsList)
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

    ~PlanStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
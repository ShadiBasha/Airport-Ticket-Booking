using System.Globalization;
using System.Net;
using CsvHelper;
using ServiceStack;

namespace AirportTicketBooking;

public class PlanStorage : Plan, IFileReader, IFileWriter
{
    private static PlanStorage? _planStorage = null;
    private string _path;
    private Dictionary<int,PlanDetails> _plansDetailsMap;
    public bool SaveDataBeforeClosing {
        get;
        set;
    }

    private PlanStorage(string path)
    {
        _path = path;
        _plansDetailsMap = new Dictionary<int, PlanDetails>();
    }

    public bool Add(PlanDetails planDetails)
    {
        _plansDetailsMap.Add(planDetails.Id,planDetails);
        return true;
    }

    public bool FindPlan(int id)
    {
        if (_plansDetailsMap.ContainsKey(id))
        {
            return true;
        }
        return false;
    }

    public static PlanStorage GetStorageInstance(string path = "PlanData.csv")
    {
        if (_planStorage == null)
        {
            
            _planStorage = new PlanStorage(path);
        }
        
        return _planStorage;
    }
    
    public bool ReadFile()
    {
        if (File.Exists(_path))
        {
            string data = File.ReadAllText(_path);
            List<PlanDetails> planDetailsList = data.FromCsv<List<PlanDetails>>();
            Plan.IdGenerator = planDetailsList[^1].Id + 1;
            _plansDetailsMap = planDetailsList
                .Select(plan => new { plan.Id, plan })
                .ToDictionary(x => x.Id,x=> x.plan);
            return true;
        }
        return false;
    }
    
    public bool WriteInFile()
    {
        if (!File.Exists(_path))
        {
            _path = "PlanData.csv";
        }

        List<PlanDetails> plansDetails = _plansDetailsMap.Values.ToList();
        File.WriteAllText(_path,plansDetails.ToCsv());
        return true;
    }

    ~PlanStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }

    public override string ToString()
    {
        string data ="****************************";
        foreach (var planDetails in _plansDetailsMap)
        {
            data += $"""
                    
                    {planDetails.Value}
                    ****************************   
                    """;
        }
        return data;
    }
}
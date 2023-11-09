using System.Globalization;
using System.Net;
using CsvHelper;
using ServiceStack;

namespace AirportTicketBooking;

public class PlanStorage : Plan, IFileReader, IFileWriter
{
    private static PlanStorage? _planStorage = null;
    private string _path;
    private List<PlanDetails> _plansDetailsList;
    public bool SaveDataBeforeClosing {
        get;
        set;
    }

    private PlanStorage(string path)
    {
        _path = path;
        _plansDetailsList = new List<PlanDetails>();
    }

    public bool AddPlan(PlanDetails planDetails)
    {
        _plansDetailsList.Add(planDetails);
        return true;
    }

    public static PlanStorage GetStorageInstanse(string path = "PlanData.csv")
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
            _plansDetailsList = data.FromCsv<List<PlanDetails>>();
            Plan.IdGenerator = _plansDetailsList[^1].Id + 1;
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
        File.WriteAllText(_path,_plansDetailsList.ToCsv());
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
        foreach (var planDetails in _plansDetailsList)
        {
            data += $"""
                    
                    {planDetails}
                    ****************************   
                    """;
        }
        return data;
    }
}
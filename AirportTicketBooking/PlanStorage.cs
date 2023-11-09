using System.Globalization;
using System.Net;
using CsvHelper;
using ServiceStack;

namespace AirportTicketBooking;

public class PlanStorage : IFileReader, IFileWriter
{
    private static PlanStorage? _planStorage = null;
    private string _path;
    private List<Plan> _plansData;
    public bool SaveDataBeforeClosing {
        get;
        set;
    }

    private PlanStorage(string path)
    {
        _path = path;
        _plansData = new List<Plan>();
    }

    public bool AddPlan(Plan plan)
    {
        _plansData.Add(plan);
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
            _plansData = data.FromCsv<List<Plan>>();
            Plan.IdGenerator = _plansData[^1].Id + 1;
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
        File.WriteAllText(_path,_plansData.ToCsv());
        return true;
    }

    ~PlanStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
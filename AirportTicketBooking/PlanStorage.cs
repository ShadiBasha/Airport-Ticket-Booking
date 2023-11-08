using System.Globalization;
using System.Net;
using CsvHelper;

namespace AirportTicketBooking;

public class PlanStorage : IFileReader, IFileWriter<Plan>
{
    private static PlanStorage? _planStorage = null;
    private string _path;
    private List<Plan> _plansData;

    private PlanStorage(string path)
    {
        _path = path;
        _plansData = new List<Plan>();
    }

    public PlanStorage GetStorageInstanse(string path = "")
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
            var reader = new StreamReader("filePersons.csv");
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _plansData = (List<Plan>) csv.GetRecords<Plan>();
            return true;
        }
        return false;
    }


    public bool WriteInFile(List<Plan> data)
    {
        var writer = File.AppendText("PlanData.csv");
        var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(_plansData);
        return true;
    }
}
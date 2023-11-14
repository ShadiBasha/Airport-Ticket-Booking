using AirportTicketBooking.Details;

namespace AirportTicketBooking.Storage;

public class BookingStorage : Storage<BookingDetails>
{
    private int IdGenerator { get; set; }
    private static BookingStorage? _bookingStorage = null;
    private BookingStorage(string path)
    {
        _defaultPath = "BookingData.csv";
        _path = path ?? _defaultPath;
        _dataDetailsMap = new Dictionary<int, BookingDetails>();
    }
    public int GetCurrentId()
    {
        return IdGenerator++;
    }
    public BookingDetails? FindBooking(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var booking) ? booking : null;
    }
    public static BookingStorage GetStorageInstance(string path = "BookingData.csv")
    {
        if (_bookingStorage == null)
        {
            _bookingStorage = new BookingStorage(path);
        }
        return _bookingStorage;
    }
    protected override void SetGenerator(List<BookingDetails> detailsList)
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
    ~BookingStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }

    }
}
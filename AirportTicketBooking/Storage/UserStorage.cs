using AirportTicketBooking.Details;
using AirportTicketBooking.Security;

namespace AirportTicketBooking.Storage;

public class UserStorage : Storage<UserDetails>
{
    private int IdGenerator { get; set; }
    private static UserStorage? _userStorage = null;
    private UserStorage(string? path)
    {
        _defaultPath = "UserData.csv";
        _path = path ?? _defaultPath;
        IdGenerator = 0;
        _dataDetailsMap = new Dictionary<int, UserDetails>();
    }
    public int GetCurrentId()
    {
        return IdGenerator++;
    }

    public UserDetails? FindUser(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var user) ? user : null;
    }
    public static UserStorage GetStorageInstance(string? path = null)
    {
        if (_userStorage == null)
        {
            _userStorage = new UserStorage(path);
        }
        return _userStorage;
    }
    protected override void SetGenerator(List<UserDetails> detailsList)
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

    public UserDetails Login(string name, string password)
    {
         UserDetails? user = _dataDetailsMap
             .FirstOrDefault(user => user.Value.Name == name 
                                     && user.Value.Password == HashGenerator.HashPassword(password, user.Value.Salt)).Value;
         if (user == null)
         {
             throw new Exception("The username or password is wrong please try again.");
         }
         return user;
    }

    ~UserStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
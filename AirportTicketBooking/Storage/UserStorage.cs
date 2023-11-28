using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Models;
using AirportTicketBooking.Security;

namespace AirportTicketBooking.Storage;

public class UserStorage : Storage<User>, IStorage
{
    public UserStorage(string path): base(path) {}
    public User? FindUser(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var user) ? user : null;
    }
    public User Login(string name, string password)
    {
         var user = _dataDetailsMap
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
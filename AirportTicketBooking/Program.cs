using ServiceStack;
using System.Security.Cryptography;
using System.Text;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        UserDetails user = new UserDetails(0, "shadi", "123");
        UserStorage us = UserStorage.GetStorageInstance();
        UserDetails shadi = us.Login("shadi", "123");
        Console.WriteLine(shadi);
        return 0;
    }
}


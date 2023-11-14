using ServiceStack;
using System.Security.Cryptography;
using System.Text;
using AirportTicketBooking.Details;
using AirportTicketBooking.Enum;
using AirportTicketBooking.Filter;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        UserStorage us = UserStorage.GetStorageInstance();
        FlightStorage fs = FlightStorage.GetStorageInstance();
        BookingStorage bs = BookingStorage.GetStorageInstance();
        AirportStorage airs = AirportStorage.GetStorageInstance();
        bs.ReadFile();
        UserDetails user = new UserDetails(us.GetCurrentId(), "fadi", "123");
        
        // us.SaveDataBeforeClosing = true;
        // fs.SaveDataBeforeClosing = true;
        // bs.SaveDataBeforeClosing = true;
        // us.AddData(user);
        // user.BookAFlight(5,Classes.Economy);
        // user.BookAFlight(2,Classes.FirstClass);
        // user.BookAFlight(3,Classes.Business);
        // us.WriteInFile();
        // fs.WriteInFile();
        // bs.WriteInFile();
        foreach (var data in BookingFilter.GetAFlightBookings(bs.GetData(),7))
        {
                Console.WriteLine($"{data.Value}");
        }
        // Console.WriteLine(bs);
        return 0;
    }
}


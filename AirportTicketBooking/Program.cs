using ServiceStack;
using System.Security.Cryptography;
using System.Text;
using AirportTicketBooking.Static_Classes;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking;

class Program
{
    public static int Main()
    {
        UserStorage us = UserStorage.GetStorageInstance();
        FlightStorage fs = FlightStorage.GetStorageInstance();
        // UserDetails user = new UserDetails(us.GetCurrentId(), "salim", "123");
        // us.AddData(user);
        // user.BookAFlight(1,Classes.Economy);
        // user.BookAFlight(2,Classes.FirstClass);
        // user.BookAFlight(3,Classes.Economy);
        // us.WriteInFile();
        
        foreach (var data in BookingFilter.GetBookingInPriceRange(us,fs,100,20000))
        {
                Console.WriteLine($"{data.Item1} --- {data.Item2} --- {data.Item3} --- {data.Item4}");
        }

        Console.WriteLine(fs);
        return 0;
    }
}


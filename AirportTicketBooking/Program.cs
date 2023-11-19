﻿using System.Reflection;
using ServiceStack;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using AirportTicketBooking.Details;
using AirportTicketBooking.Enum;
using AirportTicketBooking.Filter;
using AirportTicketBooking.Storage;

namespace AirportTicketBooking;

class Program
{
        private static readonly PlanStorage PlanStorageInstance = PlanStorage.GetStorageInstance();
        private static readonly FlightStorage FlightStorageInstance = FlightStorage.GetStorageInstance();
        private static readonly AirportStorage AirportStorageInstance = AirportStorage.GetStorageInstance();
        private static readonly BookingStorage BookingStorageInstance = BookingStorage.GetStorageInstance();
        private static readonly UserStorage UserStorageInstance = UserStorage.GetStorageInstance();
        private static UserDetails? _currentUser;
    static void InitiateStorage()
    {
        PlanStorageInstance.ReadFile();
        FlightStorageInstance.ReadFile();
        AirportStorageInstance.ReadFile();
        BookingStorageInstance.ReadFile();
        UserStorageInstance.ReadFile();
    }
    private static string FormatData<T>(Dictionary<int, T> _dataDetailsMap)
    {
        string data ="****************************";
        foreach (var details in _dataDetailsMap)
        {
            data += $"""

                     {details.Value}
                     ****************************
                     """;
        }
        return data;
    }   
    private static void PrintEnumOptions<T>()
    {
        Console.WriteLine("Available options:");
        
        foreach (var enumValue in System.Enum.GetValues(typeof(T)))
        {
            Console.WriteLine($"{(int)enumValue} - {enumValue}");
        }
    }
    static void SearchForFlight()
    {
        int? maxPrice = null;
        int? minPrice = null;
        Country? departureCountry = null;
        Country? destinationCountry = null;
        DateTime? departureDate = null;
        int? departureAirport = null;
        int? arrivalAirport = null;
        Classes? classType = Classes.Economy;
        Dictionary<int, FlightDetails> flightDetailsMap = FlightStorageInstance.GetData();
        string? command;
        Console.WriteLine(FormatData(flightDetailsMap));
        do
        {
            Console.WriteLine($"""
                               Filter Flights
                               1 - Price Range         : {minPrice}  {maxPrice}
                               2 - Departure Country   : {departureCountry}
                               3 - Destination Country : {destinationCountry}
                               4 - Departure Date      : {departureDate}
                               5 - Departure Airport   : {AirportStorageInstance.FindAirport(departureAirport)?.Name}
                               6 - Arrival Airport     : {AirportStorageInstance.FindAirport(arrivalAirport)?.Name}
                               7 - Book Flight.
                               C - Clear Filters
                               E - Exit
                               """);
            command = Console.ReadLine();
            if (command == "1")
            {
                int min = -1;
                int max = Int32.MaxValue;
                Console.WriteLine("Enter minimum price:");
                if (int.TryParse(Console.ReadLine(), out int enteredMinPrice))
                {
                    min = enteredMinPrice;
                }
                else
                {
                    Console.WriteLine("Invalid input for Minimum price. Please enter a valid integer.");
                }

                Console.WriteLine("Enter maximum price:");
                if (int.TryParse(Console.ReadLine(), out int enteredMaxPrice))
                {
                    max = enteredMaxPrice;
                }
                else
                {
                    Console.WriteLine("Invalid input for Maximum price. Please enter a valid integer.");
                }

                if (min != -1 && max != Int32.MaxValue)
                {
                    minPrice = min;
                    maxPrice = max;
                }
            }
            else if (command == "2")
            {
                PrintEnumOptions<Country>();
                Console.WriteLine("Enter the Country ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDepartureCountry))
                {
                    departureCountry = (Country)enteredDepartureCountry;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure country. Please enter a valid integer.");
                }
            }
            else if (command == "3")
            {
                PrintEnumOptions<Country>();
                Console.WriteLine("Enter the Country ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDestinationCountry))
                {
                    destinationCountry = (Country)enteredDestinationCountry;
                }
                else
                {
                    Console.WriteLine("Invalid input for Destination country. Please enter a valid integer.");
                }
            }
            else if (command == "4")
            {
                Console.WriteLine("Enter departure date (YYYY-MM-DD):");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime enteredDepartureDate))
                {
                    departureDate = enteredDepartureDate;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure date. Please enter a valid date.");
                }
            }
            else if (command == "5")
            {
                Console.WriteLine(AirportStorageInstance);
                Console.WriteLine("Enter the departure airport ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDepartureAirport))
                {
                    departureAirport = enteredDepartureAirport;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure airport. Please enter a valid integer.");
                }
            }
            else if (command == "6")
            {
                Console.WriteLine(AirportStorageInstance);
                Console.WriteLine("Enter the arrival airport ID");
                if (int.TryParse(Console.ReadLine(), out int enteredArrivalAirport))
                {
                    arrivalAirport = enteredArrivalAirport;
                }
                else
                {
                    Console.WriteLine("Invalid input for Arrival airport. Please enter a valid integer.");
                }
            }
            else if (command == "7")
            {
                Console.WriteLine("Enter the Flight ID That you want to book");
                int flightId = -1;
                Classes? flightClassType = null;
                if (int.TryParse(Console.ReadLine(), out int enteredFlightId))
                {
                     flightId = enteredFlightId;
                }
                else
                {
                    Console.WriteLine("Invalid input for Flight ID. Please enter a valid integer.");
                }
                PrintEnumOptions<Classes>();
                Console.WriteLine("Enter the Class");
                if (int.TryParse(Console.ReadLine(), out int enteredClass))
                {
                    flightClassType = (Classes)enteredClass;
                }
                else
                {
                    Console.WriteLine("Invalid input for Class ID. Please enter a valid integer.");
                }
                if(flightId != -1 && flightClassType != null)
                    try
                    {
                        _currentUser?.BookAFlight(flightId, (Classes)flightClassType);
                        Console.Clear();
                        Console.WriteLine($"""
                                          Flight was booked successfully
                                          Booking details 
                                          Booking class : {flightClassType}
                                          Flight details
                                          {FlightStorageInstance.FindFlight(flightId)}
                                          """);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                BookingStorageInstance.WriteInFile();
                UserStorageInstance.WriteInFile();
                continue;
            }
            else if (command == "C")
            {
                minPrice = null;
                maxPrice = null;
                departureCountry = null;
                destinationCountry = null;
                departureDate = null;
                departureAirport = null;
                arrivalAirport = null;
                classType = null;
                Console.WriteLine("Filters cleared.");
            }
            else if (command == "E" || command == "e")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command. Please enter a valid option.");
                continue;
            }

            foreach (var flight in FlightFilter.FilterByAll(flightDetailsMap,departureDate,departureAirport,arrivalAirport,departureCountry,destinationCountry,minPrice,maxPrice,classType)) 
            {
                Console.Clear();
                Console.WriteLine("Results : ");
                Console.WriteLine(flight);
                Console.WriteLine("""
                                  N: next
                                  E: Exit
                                  """);
                string? nextExit;
                nextExit = Console.ReadLine();
                if (nextExit == "E")
                {
                    break;
                }
            }
        } while (true);
        Console.Clear();
        UserPage();
    }
    static void ManageBooking()
    {
        string? command;
        do
        {
            Console.WriteLine("""
                              Manage Bookings
                              1 - View personal bookings
                              2 - Modify a booking
                              3 - Cancel a booking
                              E - Exit
                              """);
            command = Console.ReadLine();
            if (command == "1")
            {
                Console.WriteLine("****************************");
                foreach (var bookingId in _currentUser!.BookingIds)
                {
                    Console.WriteLine(BookingStorageInstance.FindBooking(bookingId));
                }
            }
            else if (command == "2")
            {
                int bookingID;
                Console.WriteLine("Enter the Booking ID");
                if (int.TryParse(Console.ReadLine(), out int enteredBookingId))
                {
                    bookingID = enteredBookingId;
                }
                else
                {
                    Console.WriteLine("Invalid input for Booking ID. Please enter a valid integer.");
                    continue;
                }
                PrintEnumOptions<Classes>();
                int newClassType;
                Console.WriteLine("Enter the New Class ID");
                if (int.TryParse(Console.ReadLine(), out int enteredNewClassType))
                {
                    newClassType = enteredNewClassType;
                }
                else
                {
                    Console.WriteLine("Invalid input for Class type. Please enter a valid integer.");
                    continue;
                }

                try
                {
                    _currentUser!.ModifyBooking(bookingID,(Classes)newClassType);
                    UserStorageInstance.WriteInFile();
                    BookingStorageInstance.WriteInFile();
                    Console.WriteLine("Booking has been modified");
                    Console.WriteLine(BookingStorageInstance.FindBooking(bookingID));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (command == "3")
            {
                int bookingID;
                Console.WriteLine("Enter the Booking ID");
                if (int.TryParse(Console.ReadLine(), out int enteredBookingId))
                {
                    bookingID = enteredBookingId;
                }
                else
                {
                    Console.WriteLine("Invalid input for Booking ID. Please enter a valid integer.");
                    continue;
                }
                try
                {
                    _currentUser!.CancelBooking(bookingID);
                    UserStorageInstance.WriteInFile();
                    BookingStorageInstance.WriteInFile();
                    Console.WriteLine("Booking has been Canceled");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
            else if(command == "E" || command == "e")
            {
                Console.Clear();
                UserPage();
                break;
            }
        } while (true);

    }
    static void UserPage()
    {
        Console.WriteLine($"""
                          User ID : {_currentUser?.Id}
                          Username : {_currentUser?.Name}
                          1 - Search/Book a flight
                          2 - Manage bookings
                          Type the number for the command
                          Type anything else to exit.
                          """);
        string? command = Console.ReadLine();
        if (command == "1")
        {
            SearchForFlight();
        }
        else if (command == "2")
        {
            ManageBooking();
        }
        else
        {
            _currentUser = null;
            Console.Clear();
            LandingPage();
        }
    }
    static void FilterBookings()
    {
        int? flightId = null;
        int? userId = null;
        int? maxPrice = null;
        int? minPrice = null;
        Country? departureCountry = null;
        Country? destinationCountry = null;
        DateTime? departureDate = null;
        int? departureAirport = null;
        int? arrivalAirport = null;
        Classes? classType = null;
        string? command;
        Console.WriteLine(FormatData(BookingStorageInstance.GetData()));
        do
        {
            Console.WriteLine($"""
                               Filter Bookings
                               1 - Flight ID           : {flightId}
                               2 - Price               : {minPrice}  {maxPrice}
                               3 - Departure Country   : {departureCountry}
                               4 - Destination Country : {destinationCountry}
                               5 - Departure Date      : {departureDate}
                               6 - Departure Airport   : {AirportStorageInstance.FindAirport(departureAirport)?.Name}
                               7 - Arrival Airport     : {AirportStorageInstance.FindAirport(arrivalAirport)?.Name}
                               8 - Passenger           : {userId}
                               9 - Class               : {classType}
                               C - Clear Filters
                               E - Exit
                               """);
            command = Console.ReadLine();
            if (command == "1")
            {
                Console.WriteLine(FlightStorageInstance);
                Console.WriteLine("Enter the Flight ID");
                if (int.TryParse(Console.ReadLine(), out int enteredFlightId))
                {
                    flightId = enteredFlightId;
                }
                else
                {
                    Console.WriteLine("Invalid input for Flight ID. Please enter a valid integer.");
                } 
            }
            else if (command == "2")
            {
                int min = -1;
                int max = Int32.MaxValue;
                Console.WriteLine("Enter minimum price:");
                if (int.TryParse(Console.ReadLine(), out int enteredMinPrice))
                {
                    min = enteredMinPrice;
                }
                else
                {
                    Console.WriteLine("Invalid input for Minimum price. Please enter a valid integer.");
                }

                Console.WriteLine("Enter maximum price:");
                if (int.TryParse(Console.ReadLine(), out int enteredMaxPrice))
                {
                    max = enteredMaxPrice;
                }
                else
                {
                    Console.WriteLine("Invalid input for Maximum price. Please enter a valid integer.");
                }

                if (min != -1 && max != Int32.MaxValue)
                {
                    minPrice = min;
                    maxPrice = max;
                }
            }
            else if (command == "3")
            {
                PrintEnumOptions<Country>();
                Console.WriteLine("Enter the Country ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDepartureCountry))
                {
                    departureCountry = (Country)enteredDepartureCountry;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure country. Please enter a valid integer.");
                }
            }
            else if (command == "4")
            {
                PrintEnumOptions<Country>();
                Console.WriteLine("Enter the Country ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDestinationCountry))
                {
                    destinationCountry = (Country)enteredDestinationCountry;
                }
                else
                {
                    Console.WriteLine("Invalid input for Destination country. Please enter a valid integer.");
                }
            }
            else if (command == "5")
            {
                Console.WriteLine("Enter departure date (YYYY-MM-DD):");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime enteredDepartureDate))
                {
                    departureDate = enteredDepartureDate;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure date. Please enter a valid date.");
                }
            }
            else if (command == "6")
            {
                Console.WriteLine(AirportStorageInstance);
                Console.WriteLine("Enter the departure airport ID");
                if (int.TryParse(Console.ReadLine(), out int enteredDepartureAirport))
                {
                    departureAirport = enteredDepartureAirport;
                }
                else
                {
                    Console.WriteLine("Invalid input for Departure airport. Please enter a valid integer.");
                }
            }
            else if (command == "7")
            {
                Console.WriteLine(AirportStorageInstance);
                Console.WriteLine("Enter the arrival airport ID");
                if (int.TryParse(Console.ReadLine(), out int enteredArrivalAirport))
                {
                    arrivalAirport = enteredArrivalAirport;
                }
                else
                {
                    Console.WriteLine("Invalid input for Arrival airport. Please enter a valid integer.");
                }
            }
            else if (command == "8")
            {
                Console.WriteLine(UserStorageInstance);
                Console.WriteLine("Enter the Passenger ID");
                if (int.TryParse(Console.ReadLine(), out int enteredUserId))
                {
                     userId = enteredUserId;
                }
                else
                {
                    Console.WriteLine("Invalid input for User ID. Please enter a valid integer.");
                }
            }
            else if (command == "9")
            {
                PrintEnumOptions<Classes>();
                Console.WriteLine("Enter the Class ID");
                if (int.TryParse(Console.ReadLine(), out int enteredClass))
                {
                    classType = (Classes)enteredClass;
                }
                else
                {
                    Console.WriteLine("Invalid input for Class type ID. Please enter a valid integer.");
                }
            }
            else if (command == "C")
            {
                flightId = null;
                userId = null;
                minPrice = null;
                maxPrice = null;
                departureCountry = null;
                destinationCountry = null;
                departureDate = null;
                departureAirport = null;
                arrivalAirport = null;
                classType = null;
                Console.WriteLine("Filters cleared.");
            }
            else if (command == "E" || command == "e")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command. Please enter a valid option.");
                continue;
            }
            foreach (var booking in BookingFilter.FilterByAll(BookingStorageInstance.GetData(),FlightStorageInstance,AirportStorageInstance,flightId,minPrice,maxPrice,departureCountry,destinationCountry,departureAirport,arrivalAirport,userId,classType))
            {
                Console.Clear();
                Console.WriteLine("Results : ");
                Console.WriteLine(booking);
                Console.WriteLine("""
                                  N: next
                                  E: Exit
                                  """);
                string? nextExit;
                nextExit = Console.ReadLine();
                if (nextExit == "E")
                {
                    break;
                }
            }
        } while (true);
        Console.Clear();
        AdminPage();
    }
    static void UploadFlights()
    {
        string? command;
        do
        {
            Console.WriteLine("""
                              Upload flights
                              1 - Show manual
                              2 - Show Plans
                              3 - Show Airports
                              4 - Upload Flight
                              E - Exit
                              """);
            command = Console.ReadLine();
            if (command == "1")
            {
                Console.Clear();
                Type type = typeof(FlightDetails);
                Console.WriteLine("--------------------------------");
                foreach (var property in type.GetProperties())
                {
                    Console.WriteLine($"""
                                      Property Name : {property.Name}
                                      Property Type : {property.PropertyType}
                                      --------------------------------
                                      """);
                }

                Console.WriteLine("""
                                  Note :
                                  - Takeoff Time should be in this format YYYY-MM-DD HH:MM:SS.
                                  - Airports ID should be for an existing Airport.
                                  - Plan ID should be for an existing Plan.
                                  - Data should be Entered in this 
                                  --> (Plan ID, Departure Airport ID, Arrival Airport ID, Takeoff Time, Duration)
                                  """);
            }
            else if (command == "2")
            {
                Console.WriteLine(PlanStorageInstance);
            }
            else if (command == "3")
            {
                Console.WriteLine(AirportStorageInstance);
            }
            else if (command == "4")
            {
                Console.Clear();
                Console.WriteLine("File Path");
                string? path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("Error : File does not exist please check the path and try again");
                    continue;
                }
                try
                {  
                    FlightStorageInstance.WriteFromAUserFile(path);
                    Console.WriteLine("Flights are Uploaded successfully");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (command == "E" || command == "e")
            {
                break;
            }
        } while (true);
        Console.Clear();
        AdminPage();
    }
    static void AdminPage()
    {
        Console.WriteLine($"""
                           Welcome to the Admin page
                           1 - Filter Bookings
                           2 - Upload flights
                           Type the number for the command
                           Type anything else to exit.
                           """);
        string? command = Console.ReadLine();
        if (command == "1")
        {
            Console.Clear();
            FilterBookings();
        }
        else if (command == "2")
        {
            Console.Clear();
            UploadFlights();
        }
        else
        {
            Console.Clear();
            LandingPage();
        }
    }
    static void LoggingPage()
    {
        Console.WriteLine("Username: ");
        string? username = Console.ReadLine();
        Console.WriteLine("Password: ");
        string? password = Console.ReadLine();
        if (username == string.Empty || password == string.Empty)
        {
            Console.Clear();
            Console.WriteLine("The username and password can not be null try again");
            LandingPage();
        }

        try
        {
            _currentUser = UserStorageInstance.Login(username, password);
            Console.Clear();
            UserPage();
        }
        catch (Exception e)
        {
            Console.Clear(); 
            Console.WriteLine(e.Message);
            LandingPage();
        }
    }
    static void AdminLogin()
    {
        Console.WriteLine("Password");
        string? password = Console.ReadLine();
        if (password == "123")
        {
            Console.Clear();
            AdminPage();
        }
        else
        {
            Console.WriteLine("The password is wrong");
        }
    }
    static void LandingPage()
    {
        Console.WriteLine("""
                          Welcome to the Airport Ticket Booking System
                          1 -                Log in 
                          2 -             Admin Log in
                          E -                 Exit
                          """);
        string? command = Console.ReadLine();
        if (command == "1")
        {
            Console.Clear();
            LoggingPage();
        }else if (command == "2")
        {
            Console.Clear();
            AdminLogin();
        }
        else if (command == "E")
        {
            Console.WriteLine("Closing the application...");
            PlanStorageInstance.WriteInFile();
            FlightStorageInstance.WriteInFile();
            AirportStorageInstance.WriteInFile(); 
            BookingStorageInstance.WriteInFile();
            UserStorageInstance.WriteInFile();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Please enter a valid command\n");
            LandingPage();
        }
    }
    public static int Main()
    {
        InitiateStorage();
        LandingPage();
        return 0;
    }
}


using AirportTicketBooking.Enum;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking.Models;
using ServiceStack;

namespace AirportTicketBooking.Storage;

public class FlightStorage : Storage<Flight>, IStorage
{ 
    public FlightStorage(string path): base(path) {}
    public Flight? FindFlight(int id)
    {
        return _dataDetailsMap.TryGetValue(id, out var flight) ? flight : null;
    }
    public void WriteFromAUserFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception("Error: Can't access the file Check the file path and try again");
        }
        var flightStorage = StorageFactory.GetStorage(StorageType.Flight) as FlightStorage;
        var planStorage = StorageFactory.GetStorage(StorageType.Plan) as PlanStorage;
        var airportStorage = StorageFactory.GetStorage(StorageType.Airport) as AirportStorage;
        var newFlights = new List<Flight>();
        var reader = File.OpenRead(path);
        var lines = reader.ReadLines();

        foreach (var line in lines)
        {
            int index = 0;
            var fields = line.Split(',');
            if (fields.Length > 5)
            {
                throw new Exception("Error: Extra data is provided please check the manual and try again");
            }
            
            int planId;
            int departureAirportId;
            int arrivalAirportId;
            DateTime takeoffTime;
            TimeOnly duration;

            try
            {
                planId = int.Parse(fields[0]);
                if (planStorage.FindPlan(planId) == null)
                {
                    throw new Exception("Error: plan does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the plan id field");
            }

            try
            {
                departureAirportId = int.Parse(fields[1]);
                if (airportStorage.FindAirport(departureAirportId) == null)
                {
                    throw new Exception("Error: Airport does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the departure airport field");
            }

            try
            {
                arrivalAirportId = int.Parse(fields[2]);
                if (airportStorage.FindAirport(arrivalAirportId) == null)
                {
                    throw new Exception("Error: Airport does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is integer in the arrival airport field");
            }
            
            try
            {
                takeoffTime = DateTime.Parse(fields[3]);
                if (takeoffTime < DateTime.Today)
                {
                    throw new Exception($"Error in Line {index}: The data provided is in the past change the data and try again");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception($"Error in Line {index}: Make sure the value is Data and time in the takeoff time field");
            }
            
            try
            {
                duration = TimeOnly.Parse(fields[4]);
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Line {index}: Make sure the value is a positive time in the duration field");
            }
            Flight newFlight =
                new Flight(GetCurrentId(),takeoffTime, planId, duration, departureAirportId, arrivalAirportId);
            newFlights.Add(newFlight);
        }
        
        foreach (var flight in newFlights)
        {
            IdGenerator++;
            _dataDetailsMap.Add(IdGenerator, flight);
        }
        flightStorage?.WriteInFile();
    }
    ~FlightStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
    public Dictionary<int, Flight> FilterFlightInPriceRange(int minPrice, int maxPrice, Classes classType = Classes.Economy)
    {
        return _dataDetailsMap
            .Values
            .Where(flight => flight.ComputePrices()[(int)classType] >= minPrice && flight.ComputePrices()[(int)classType] <= maxPrice)
            .ToDictionary(flight => flight.Id);
    }

    public Dictionary<int,Flight> FilterById(int id)
    {
        return _dataDetailsMap
            .Where(flight => flight.Key == id)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    } 
    
    public Dictionary<int,Flight> FilterByDepartureDate(DateTime dateTime)
    {
        return _dataDetailsMap
            .Where(flight => flight.Value.TakeoffTime >= dateTime)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    }   
    
   public  Dictionary<int,Flight> FilterByDepartureAirport(int departureAirport)
   {
        return _dataDetailsMap
            .Where(flight => flight.Value.DepartureAirportId == departureAirport)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    }   
   
   public Dictionary<int,Flight> FilterByArrivalAirport(int arrivalAirport)
   {
       return _dataDetailsMap
           .Where(flight => flight.Value.ArrivalAirportId == arrivalAirport)
           .ToDictionary(flight => flight.Key, flight => flight.Value);
   }
   
   public Dictionary<int,Flight> FilterByDepartureCountry(AirportStorage airportStorage,Country departureCountry)
   {
       return airportStorage.GetData()
           .Where(airport => airport.Value.AirportCountry == departureCountry)
           .Join(_dataDetailsMap, airport => airport.Value.Id,
               flight => flight.Value.DepartureAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }
   
   public Dictionary<int,Flight> FilterByDestinationCountry(AirportStorage airportStorage,Country destinationCountry)
   {
       return airportStorage.GetData()
           .Where(airport => airport.Value.AirportCountry == destinationCountry)
           .Join(_dataDetailsMap, airport => airport.Value.Id,
               flight => flight.Value.ArrivalAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }

   public IEnumerable<Flight> FilterByAll(AirportStorage airportStorage,
       DateTime? dateTime, int? departureAirport, int? arrivalAirport, Country? departureCountry,
       Country? destinationCountry, int? minPrice, int? maxPrice, Classes? classType = Classes.Economy)
   {
       var flightStorage = _dataDetailsMap.CreateCopy();
       flightStorage = dateTime != null ? 
           FilterByDepartureDate((DateTime)dateTime) : flightStorage;
       flightStorage = departureAirport != null ? 
           FilterByDepartureAirport((int)departureAirport) : flightStorage;
       flightStorage = arrivalAirport != null ? 
           FilterByArrivalAirport((int)arrivalAirport) : flightStorage;
       flightStorage = departureCountry != null ?
           FilterByDepartureCountry(airportStorage,(Country)departureCountry) : flightStorage;
       flightStorage = destinationCountry != null ?
           FilterByDestinationCountry(airportStorage,(Country)destinationCountry) : flightStorage;
       flightStorage = minPrice != null && maxPrice != null && classType != null ?
           FilterFlightInPriceRange((int)minPrice, (int) maxPrice, (Classes)classType) : flightStorage;
       foreach (var flight in flightStorage)
       {
           yield return flight.Value;
       }
   }
}
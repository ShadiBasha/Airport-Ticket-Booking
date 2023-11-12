namespace AirportTicketBooking;

public static class FlightFilter
{
    public static int NumberOfResults { get; set; }
    private static AirportStorage _airportStorage;
    static FlightFilter()
    {
        NumberOfResults = 5;
        _airportStorage = AirportStorage.GetStorageInstance();
        _airportStorage.ReadFile();
    }
    
    public static Dictionary<int,FlightDetails> FilterById(Dictionary<int, FlightDetails> flightStorage,int id)
    {
        return flightStorage
            .Where(flight => flight.Key == id)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    } 
    
    public static Dictionary<int,FlightDetails> FilterByDepartureDate(Dictionary<int, FlightDetails> flightStorage,DateTime dateTime)
    {
        return flightStorage
            .Where(flight => flight.Value.TakeoffTime >= dateTime)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    }   
    
   public static Dictionary<int,FlightDetails> FilterByDepartureAirport(Dictionary<int, FlightDetails> flightStorage,int departureAirport)
   {
        return flightStorage
            .Where(flight => flight.Value.DepartureAirportId >= departureAirport)
            .ToDictionary(flight => flight.Key, flight => flight.Value);
    }   
   
   public static Dictionary<int,FlightDetails> FilterByArrivalAirport(Dictionary<int, FlightDetails> flightStorage,int arrivalAirport)
   {
       return flightStorage
           .Where(flight => flight.Value.ArrivalAirportId >= arrivalAirport)
           .ToDictionary(flight => flight.Key, flight => flight.Value);
   }
   
   public static Dictionary<int,FlightDetails> FilterByDepartureCountry(Dictionary<int, FlightDetails> flightStorage,Country departureCountry)
   {

       return _airportStorage.getAirports()
           .Where(airport => airport.Value.AirportCountry == departureCountry)
           .Join(flightStorage, airport => airport.Value.Id,
               flight => flight.Value.DepartureAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }
   
   public static Dictionary<int,FlightDetails> FilterByDestinationCountry(Dictionary<int, FlightDetails> flightStorage,Country destinationCountry)
   {
       return _airportStorage.getAirports()
           .Where(airport => airport.Value.AirportCountry == destinationCountry)
           .Join(flightStorage, airport => airport.Value.Id,
               flight => flight.Value.ArrivalAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }
}
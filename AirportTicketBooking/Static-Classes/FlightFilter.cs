namespace AirportTicketBooking;

public static class FlightFilter
{
    public static int NumberOfResults { get; set; }
    private static readonly AirportStorage AirportStorage;
    static FlightFilter()
    {
        NumberOfResults = 5;
        AirportStorage = AirportStorage.GetStorageInstance();
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

       return AirportStorage.GetData()
           .Where(airport => airport.Value.AirportCountry == departureCountry)
           .Join(flightStorage, airport => airport.Value.Id,
               flight => flight.Value.DepartureAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }
   
   public static Dictionary<int,FlightDetails> FilterByDestinationCountry(Dictionary<int, FlightDetails> flightStorage,Country destinationCountry)
   {
       return AirportStorage.GetData()
           .Where(airport => airport.Value.AirportCountry == destinationCountry)
           .Join(flightStorage, airport => airport.Value.Id,
               flight => flight.Value.ArrivalAirportId,
               (airport,flight) => flight.Value)
           .ToDictionary(flight => flight.Id, flight => flight);
   }
}
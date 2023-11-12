namespace AirportTicketBooking;

public static class FlightFilter
{
    public static int NumberOfResults { get; set; }

    static FlightFilter()
    {
        NumberOfResults = 5;
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
    
   // public static Dictionary<int,FlightDetails> FilterByDepartureAirport(Dictionary<int, FlightDetails> flightStorage,Airport departureAirport)
   //  {
   //      return flightStorage
   //          .Where(flight => flight.Value.DepartureAirportId >= departureAirport)
   //          .ToDictionary(flight => flight.Key, flight => flight.Value);
   //  }   
   //
   // public static Dictionary<int,FlightDetails> FilterByArrivalAirport(Dictionary<int, FlightDetails> flightStorage,Airport arrivalAirport)
   // {
   //     return flightStorage
   //         .Where(flight => flight.Value.ArrivalAirportId >= arrivalAirport)
   //         .ToDictionary(flight => flight.Key, flight => flight.Value);
   // }
   
}
namespace AirportTicketBooking;

public interface IFileReader<T>
{
    public List<T> ReadFile(string path);
}
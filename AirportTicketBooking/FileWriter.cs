namespace AirportTicketBooking;

public interface IFileWriter<T>
{
    public bool WriteInFile(List<T> data);
}
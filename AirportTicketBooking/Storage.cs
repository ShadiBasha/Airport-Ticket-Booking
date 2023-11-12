namespace AirportTicketBooking;

public interface IStorage<out T> : IFileReader, IFileWriter
{
    public T GetStorageInstance();
    public T GetData();
    public void Add();
    public void Delete();
}
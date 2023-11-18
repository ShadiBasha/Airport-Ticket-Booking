﻿using AirportTicketBooking.Details;

namespace AirportTicketBooking.Storage;

public class AirportStorage : Storage<AirportDetails>
{
    private int IdGenerator { get; set; }
    private static AirportStorage? _airportStorage = null;

    private AirportStorage(string path)
    {
        _defaultPath = "AirportData.csv";
        _path = path ?? _defaultPath;
        _dataDetailsMap = new Dictionary<int, AirportDetails>();
    }
    
    public int GetCurrentId()
    {
        return IdGenerator++;
    }
    
    public AirportDetails? FindAirport(int? id)
    {
        if(id != null)
            return _dataDetailsMap.TryGetValue((int)id, out var airport) ? airport : null;
        return null;
    }

    public static AirportStorage GetStorageInstance(string path = "AirportData.csv")
    {
        if (_airportStorage == null)
        {
            _airportStorage = new AirportStorage(path);
        }
        return _airportStorage;
    }

    protected override void SetGenerator(List<AirportDetails> detailsList)
    {
        try
        {
            IdGenerator = detailsList[^1].Id + 1;
        }
        catch (Exception e)
        {
            IdGenerator = 0;
        }    
    }

    ~AirportStorage()
    {
        if (SaveDataBeforeClosing)
        {
            WriteInFile();
        }
    }
}
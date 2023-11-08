﻿namespace AirportTicketBooking;

public class Plan
{
    private static int _id; 
    public int Id { get; init; }
    public string Name { get; set; }
    private int _economyCapacity;
    private int _businessCapacity;
    private int _firstClassCapacity;

    public int EconomyCapacity
    {
        get => _economyCapacity;
        set
        {
            if (value > 0)
            {
                _economyCapacity = value;
            }
            _economyCapacity = 0;
        }
    }
    public int BusinessCapacity
    {
        get => _businessCapacity;
        set
        {
            if (value > 0)
            {
                _businessCapacity = value;
            }
            _businessCapacity = 0;
        }
    }
    public int FirstClassCapacity
    {
        get => _firstClassCapacity;
        set
        {
            if (value > 0)
            {
                _firstClassCapacity = value;
            }
            _firstClassCapacity = 0;
        } 
    }
    
    public Plan(string name, int economyCapacity = 0, int businessCapacity = 0, int firstClassCapacity = 0)
    {
        Id = _id;
        Name = name;
        EconomyCapacity = economyCapacity;
        BusinessCapacity = businessCapacity;
        FirstClassCapacity = firstClassCapacity;
        _id++;
    }
    
}
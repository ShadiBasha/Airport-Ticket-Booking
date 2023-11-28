using AirportTicketBooking.Interfaces;

namespace AirportTicketBooking.Models;

public class Plan : IIndexed
{
    public int Id { get; init; }
    public string Name { get; set; }
    private int _economyCapacity;
    private int _businessCapacity;
    private int _firstClassCapacity;
    private int _economyPrice;
    private int _businessPrice;
    private int _firstClassPrice;

    public int EconomyCapacity
    {
        get => _economyCapacity;
        set
        {
            if (value > 0)
            {
                _economyCapacity = value;
            }
            else
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
            else
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
            else 
                _firstClassCapacity = 0;
        } 
    }

    public int EconomyPrice
    {
        get => _economyPrice;
        set
        {
            if (value > 0)
            {
                _economyPrice = value;
            }
            else
                _economyPrice = 0;
        }
    }
    public int BusinessPrice
    {
        get => _businessPrice;
        set
        {
            if (value > 0)
            {
                _businessPrice = value;
            }
            else
                _businessPrice = 0;
        }
    }
    public int FirstClassPrice
    {
        get => _firstClassPrice;
        set
        {
            if (value > 0)
            {
                _firstClassPrice = value;
            }
            else 
                _firstClassPrice = 0;
        } 
    }
    
    public Plan(int id,string name,Tuple<int, int> economy, Tuple<int, int> business, Tuple<int, int> firstClass)
    {
        Id = id;
        Name = name;
        EconomyCapacity = economy.Item1;
        BusinessCapacity = business.Item1;
        FirstClassCapacity = firstClass.Item1;
        EconomyPrice = economy.Item2;
        BusinessPrice = business.Item2;
        FirstClassPrice = business.Item2;
    }

    public Plan(int id, string name,int economyCapacity, int businessCapacity, int firstClassCapacity, int economyPrice, int businessPrice, int firstClassPrice)
    {
        Id = id;
        EconomyCapacity = economyCapacity;
        BusinessCapacity = businessCapacity;
        FirstClassCapacity = firstClassCapacity;
        EconomyPrice = economyPrice;
        BusinessPrice = businessPrice;
        FirstClassPrice = firstClassPrice;
        Name = name;
    }

    public override string ToString()
    {
        return $"""
                Plan {Id}
                Plan Name : {Name}
                Capacity 
                Economy     : {EconomyCapacity}
                Business    : {BusinessCapacity}
                First class : {FirstClassCapacity}
                Prices per 30 minutes 
                Economy     : {EconomyPrice}$
                Business    : {BusinessPrice}$
                First class : {FirstClassPrice}$
                """;
    }
}
namespace AirportTicketBooking;

public class PlanDetails : Plan
{
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
    
    public PlanDetails(string name, int economyCapacity = 0, int businessCapacity = 0, int firstClassCapacity = 0)
    {
        Id = IdGenerator;
        Name = name;
        EconomyCapacity = economyCapacity;
        BusinessCapacity = businessCapacity;
        FirstClassCapacity = firstClassCapacity;
        IdGenerator++;
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
                """;
    }
}
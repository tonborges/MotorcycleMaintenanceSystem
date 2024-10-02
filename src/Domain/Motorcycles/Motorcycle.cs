using Domain.Rents;
using Shared.Domain;

namespace Domain.Motorcycles;

public class Motorcycle : Entity
{
    public string? Identifier { get; private set; }
    public int? Year { get; private set; }
    public string? Model { get; private set; }
    public string? Plate { get; private set; }
    public virtual ICollection<Rent> Rents { get; private set; } = [];
    
    public Motorcycle SetIdentifier(string? identifier)
    {
        Identifier = identifier;
        return this;
    }

    public Motorcycle SetYear(int? year)
    {
        Year = year;
        return this;
    }

    public Motorcycle SetModel(string? model)
    {
        Model = model;
        return this;
    }

    public Motorcycle SetPlate(string? plate)
    {
        Plate = plate;
        return this;
    }
}
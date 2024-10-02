using Application.Abstractions.Messaging;
using Domain.Motorcycles;

namespace Application.Features.Motorcycles.Create;

public class CreateMotorcycleCommand : ICommand
{
    public string? Identifier { get; set; }
    public int? Year { get; set; }
    public string? Model { get; set; }
    public string? Plate { get; set; }

    public static explicit operator Motorcycle(CreateMotorcycleCommand command)
    {
        return new Motorcycle()
               .SetIdentifier(command.Identifier)
               .SetYear(command.Year)
               .SetModel(command.Model)
               .SetPlate(command.Plate);
    }
}
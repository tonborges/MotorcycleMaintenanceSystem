using Application.Abstractions.Messaging;
using Domain.DeliveryPeople;

namespace Application.Features.DeliveryPeople.Create;

public class CreateDeliveryPersonCommand : ICommand
{
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? Ein { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? DriversLicenseNumber { get; set; }
    public string? DriversLicenseType { get; set; }
    public string? DriversLicenseImage { get; set; }

    public static explicit operator DeliveryPerson(CreateDeliveryPersonCommand command)
        => new DeliveryPerson()
           .SetIdentifier(command.Identifier)
           .SetName(command.Name)
           .SetEin(command.Ein)
           .SetDateOfBirth(command.DateOfBirth)
           .SetDriversLicenseNumber(command.DriversLicenseNumber)
           .SetDriversLicenseType(command.DriversLicenseType switch
           {
               "A" => Domain.DeliveryPeople.DriversLicenseType.A,
               "B" => Domain.DeliveryPeople.DriversLicenseType.B,
               "AB" => Domain.DeliveryPeople.DriversLicenseType.AB,
               "A+B" => Domain.DeliveryPeople.DriversLicenseType.AB,
               _ => Domain.DeliveryPeople.DriversLicenseType.Invalid
           })
           .SetDriversLicenseImagem(command.DriversLicenseImage);
}
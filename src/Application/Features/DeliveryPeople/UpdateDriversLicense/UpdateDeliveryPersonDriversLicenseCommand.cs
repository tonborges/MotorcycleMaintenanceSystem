using Application.Abstractions.Messaging;

namespace Application.Features.DeliveryPeople.UpdateDriversLicense;

public class UpdateDeliveryPersonDriversLicenseCommand : ICommand
{
    public string? Identifier { get; set; }
    public string? DriversLicenseImage { get; set; }
    
    public UpdateDeliveryPersonDriversLicenseCommand WithIdentifier(string? identifier)
    {
        Identifier = identifier;
        return this;
    }
}
using Application.Abstractions.Messaging;

namespace Application.Features.Motorcycles.UpdatePlate;

public class UpdateMotorcyclePlateCommand : ICommand
{
    public string? Identifier { get; set; }
    public string? Plate { get; set; }
    
    public UpdateMotorcyclePlateCommand WithIdentifier(string? identifier)
    {
        Identifier = identifier;
        return this;
    }
}
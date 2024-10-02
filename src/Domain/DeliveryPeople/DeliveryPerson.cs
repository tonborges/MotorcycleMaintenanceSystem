using Domain.Rents;
using Shared.Domain;

namespace Domain.DeliveryPeople;

public class DeliveryPerson : Entity
{
    public string? Identifier { get; private set; }
    public string? Name { get; private set; }
    public string? Ein { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? DriversLicenseNumber { get; private set; }
    public DriversLicenseType? DriversLicenseType { get; private set; }
    public string? DriversLicenseImage { get; private set; }
    public virtual ICollection<Rent> Rents { get; private set; } = [];

    public DeliveryPerson SetIdentifier(string? identifier)
    {
        Identifier = identifier;
        return this;
    }

    public DeliveryPerson SetName(string? name)
    {
        Name = name;
        return this;
    }

    public DeliveryPerson SetEin(string? ein)
    {
        Ein = ein;
        return this;
    }

    public DeliveryPerson SetDateOfBirth(DateTime? dateOfBirth)
    {
        DateOfBirth = dateOfBirth;
        return this;
    }

    public DeliveryPerson SetDriversLicenseNumber(string? driversLicenseNumber)
    {
        DriversLicenseNumber = driversLicenseNumber;
        return this;
    }

    public DeliveryPerson SetDriversLicenseType(DriversLicenseType? driversLicenseType)
    {
        DriversLicenseType = driversLicenseType;
        return this;
    }

    public DeliveryPerson SetDriversLicenseImagem(string? driversLicenseImage)
    {
        DriversLicenseImage = driversLicenseImage;
        return this;
    }

    public bool CannotRideMotorcycle()
        => !CanRideMotorcycle();

    public bool CanRideMotorcycle()
        => DriversLicenseType switch
        {
            Domain.DeliveryPeople.DriversLicenseType.A => true,
            Domain.DeliveryPeople.DriversLicenseType.AB => true,
            _ => false
        };
}
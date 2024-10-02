using Domain.DeliveryPeople;
using FluentAssertions;

namespace UnitTests.DeliveryPeople;

public class DeliveryPersonTest
{
    [Fact]
    public void DeliveryPersonCanRideWithDriversLicenseTypeCategoryA()
    {
        // Arrange
        var deliveryPerson =
            new DeliveryPerson()
                .SetIdentifier("Person-1")
                .SetName("Peter Parker")
                .SetDateOfBirth(new DateTime(2000, 04, 01))
                .SetEin("94237956000155")
                .SetDriversLicenseImagem("image.png")
                .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                .SetDriversLicenseType(DriversLicenseType.A);
        
        // Act
        var canRide = deliveryPerson.CanRideMotorcycle();
        
        // Assert
        canRide.Should().BeTrue();
    } 
    
    [Fact]
    public void DeliveryPersonCanRideWithDriversLicenseTypeCategoryAb()
    {
        // Arrange
        var deliveryPerson =
            new DeliveryPerson()
                .SetIdentifier("Person-1")
                .SetName("Peter Parker")
                .SetDateOfBirth(new DateTime(2000, 04, 01))
                .SetEin("94237956000155")
                .SetDriversLicenseImagem("image.png")
                .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                .SetDriversLicenseType(DriversLicenseType.AB);
        
        // Act
        var canRide = deliveryPerson.CanRideMotorcycle();
        
        // Assert
        canRide.Should().BeTrue();
    } 
    
    [Fact]
    public void DeliveryPersonCannotRide()
    {
        // Arrange
        var deliveryPerson =
            new DeliveryPerson()
                .SetIdentifier("Person-1")
                .SetName("Peter Parker")
                .SetDateOfBirth(new DateTime(2000, 04, 01))
                .SetEin("94237956000155")
                .SetDriversLicenseImagem("image.png")
                .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                .SetDriversLicenseType(DriversLicenseType.B);
        
        // Act
        var cannotRide = deliveryPerson.CannotRideMotorcycle();
        
        // Assert
        cannotRide.Should().BeTrue();
    }
}
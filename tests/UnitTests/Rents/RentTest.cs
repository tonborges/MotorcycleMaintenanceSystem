using Domain.Rents;
using FluentAssertions;

namespace UnitTests.Rents;

public class RentTest
{
    [Fact]
    public void FinalizeRentalOnTime()
    {
        // Arrange
        var rent1 = new Rent()
                    .SetPlan(7)
                    .SetMotorcycleId("Yamaha-MT-07")
                    .SetDeliveryPersonId("Person-1")
                    .SetStartDate(new DateTime(2024, 10, 01))
                    .SetEndDate(new DateTime(2024, 10, 07))
                    .SetForecastDateEnd(new DateTime(2024, 10, 07));

        // Act
        rent1.FinalizeRental(new DateTime(2024, 10, 07));

        // Assert
        rent1.DailyPrice.Should().Be(30);
        rent1.FineValue.Should().Be(0);
        rent1.TotalValue.Should().Be(210);
    }

    [Fact]
    public void FinalizeRentalBeforeTime()
    {
        // Arrange
        var rent2 = new Rent()
                    .SetPlan(7)
                    .SetMotorcycleId("Yamaha-MT-07")
                    .SetDeliveryPersonId("Person-1")
                    .SetStartDate(new DateTime(2024, 10, 01))
                    .SetEndDate(new DateTime(2024, 10, 07))
                    .SetForecastDateEnd(new DateTime(2024, 10, 07));

        // Act
        rent2.FinalizeRental(new DateTime(2024, 10, 05));

        // Assert
        rent2.DailyPrice.Should().Be(30);
        rent2.FineValue.Should().Be(12);
        rent2.TotalValue.Should().Be(162);
    }

    [Fact]
    public void FinalizeRentalAfterTime()
    {
        // Arrange
        var rent3 = new Rent()
                    .SetPlan(7)
                    .SetMotorcycleId("Yamaha-MT-07")
                    .SetDeliveryPersonId("Person-1")
                    .SetStartDate(new DateTime(2024, 10, 01))
                    .SetEndDate(new DateTime(2024, 10, 07))
                    .SetForecastDateEnd(new DateTime(2024, 10, 07));

        // Act
        rent3.FinalizeRental(new DateTime(2024, 10, 10));

        // Assert
        rent3.DailyPrice.Should().Be(30);
        rent3.FineValue.Should().Be(150);
        rent3.TotalValue.Should().Be(360);
    }
}
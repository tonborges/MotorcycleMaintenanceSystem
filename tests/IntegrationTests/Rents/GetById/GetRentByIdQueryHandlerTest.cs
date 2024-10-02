using Application.Features.Rents.GetById;
using Domain.DeliveryPeople;
using Domain.Motorcycles;
using Domain.Rents;
using FluentAssertions;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Rents.GetById;

public class GetRentByIdQueryHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldGetRentByIdExist()
    {
        // Arrange
        var dbContext = fixture.BuildDbContext(Guid.NewGuid().ToString());

        dbContext
            .Motorcycles
            .Add(
                new Motorcycle()
                    .SetIdentifier("Yamaha-MT-07")
                    .SetModel("MT-07")
                    .SetYear(2021)
                    .SetPlate("ABC-1234")
            );

        dbContext
            .DeliveryPeople
            .Add(
                new DeliveryPerson()
                    .SetIdentifier("Person-1")
                    .SetName("Peter Parker")
                    .SetDateOfBirth(new DateTime(2000, 04, 01))
                    .SetEin("94237956000155")
                    .SetDriversLicenseImagem("image.png")
                    .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                    .SetDriversLicenseType(DriversLicenseType.A)
            );

        var rent = new Rent()
                   .SetPlan(7)
                   .SetMotorcycleId("Yamaha-MT-07")
                   .SetDeliveryPersonId("Person-1")
                   .SetStartDate(DateTime.Now.AddDays(1))
                   .SetEndDate(DateTime.Now.AddDays(8))
                   .SetForecastDateEnd(DateTime.Now.AddDays(8));
        dbContext
            .Rents
            .Add(rent);

        await dbContext.SaveChangesAsync();

        var mockLogger = new Mock<ILogger<GetRentByIdQueryHandler>>();
        var handler = new GetRentByIdQueryHandler(dbContext, mockLogger.Object);
        var command = new GetRentByIdQuery(rent.Id.ToString());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<GetRentByIdResponse>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldTryGetRentByIdThatNotExists()
    {
        // Arrange
        var dbContext = fixture.BuildDbContext(Guid.NewGuid().ToString());

        dbContext
            .Motorcycles
            .Add(
                new Motorcycle()
                    .SetIdentifier("Yamaha-MT-07")
                    .SetModel("MT-07")
                    .SetYear(2021)
                    .SetPlate("ABC-1234")
            );

        dbContext
            .DeliveryPeople
            .Add(
                new DeliveryPerson()
                    .SetIdentifier("Person-1")
                    .SetName("Peter Parker")
                    .SetDateOfBirth(new DateTime(2000, 04, 01))
                    .SetEin("94237956000155")
                    .SetDriversLicenseImagem("image.png")
                    .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                    .SetDriversLicenseType(DriversLicenseType.A)
            );

        dbContext
            .Rents
            .Add(
                new Rent()
                    .SetPlan(7)
                    .SetMotorcycleId("Yamaha-MT-07")
                    .SetDeliveryPersonId("Person-1")
                    .SetStartDate(DateTime.Now.AddDays(1))
                    .SetEndDate(DateTime.Now.AddDays(8))
                    .SetForecastDateEnd(DateTime.Now.AddDays(8))
            );

        await dbContext.SaveChangesAsync();

        var mockLogger = new Mock<ILogger<GetRentByIdQueryHandler>>();
        var handler = new GetRentByIdQueryHandler(dbContext, mockLogger.Object);
        var command = new GetRentByIdQuery(Guid.NewGuid().ToString());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<GetRentByIdResponse>>();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Locação não encontrada");
    }
}
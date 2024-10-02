using Application.Features.Rents.Create;
using Domain.DeliveryPeople;
using Domain.Motorcycles;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Rents.Create;

public class CreateRentCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldRent()
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

        await dbContext.SaveChangesAsync();

        var validatorMock = new Mock<IValidator<CreateRentCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateRentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var mockLogger = new Mock<ILogger<CreateRentCommandHandler>>();
        var handler = new CreateRentCommandHandler(dbContext, validatorMock.Object, mockLogger.Object);
        var command = new CreateRentCommand
        {
            DeliveryPersonId = "Person-1",
            MotorcycleId = "Yamaha-MT-07",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            ForecastDateEnd = DateTime.Now.AddDays(2),
            Plan = 7
        };

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
    }
}
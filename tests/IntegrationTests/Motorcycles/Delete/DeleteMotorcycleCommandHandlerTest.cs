using Application.Features.Motorcycles.Delete;
using Domain.DeliveryPeople;
using Domain.Motorcycles;
using Domain.Rents;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Motorcycles.Delete;

public class DeleteMotorcycleCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldDeleteMotorcycle()
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

        await dbContext.SaveChangesAsync();

        var validatorMock = new Mock<IValidator<DeleteMotorcycleCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteMotorcycleCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var mockLogger = new Mock<ILogger<DeleteMotorcycleCommandHandler>>();
        var handler = new DeleteMotorcycleCommandHandler(dbContext, validatorMock.Object, mockLogger.Object);
        var command = new DeleteMotorcycleCommand("Yamaha-MT-07");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldTryDeleteMotorcycleThatNotExists()
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

        await dbContext.SaveChangesAsync();

        var validatorMock = new Mock<IValidator<DeleteMotorcycleCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteMotorcycleCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var mockLogger = new Mock<ILogger<DeleteMotorcycleCommandHandler>>();
        var handler = new DeleteMotorcycleCommandHandler(dbContext, validatorMock.Object, mockLogger.Object);
        var command = new DeleteMotorcycleCommand("Yamaha-MT-06");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsFailure.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Dados inválidos");
    }
}
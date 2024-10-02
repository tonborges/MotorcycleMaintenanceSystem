using Application.Features.Motorcycles.Create;
using Domain.Motorcycles;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Motorcycles.Create;

public class CreateMotorcycleCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldCreateMotorcycle()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<CreateMotorcycleCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateMotorcycleCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var mockLogger = new Mock<ILogger<CreateMotorcycleCommandHandler>>();
        var handler = new CreateMotorcycleCommandHandler(fixture.BuildDbContext(Guid.NewGuid().ToString()), validatorMock.Object, mockLogger.Object);
        var command = new CreateMotorcycleCommand
        {
            Identifier = "Yamaha-MT-07",
            Model = "MT-07",
            Year = 2021,
            Plate = "ABC-1234"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldTryCreateMotorcycleEqualAnExistingOne()
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

        var validatorMock = new Mock<IValidator<CreateMotorcycleCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateMotorcycleCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        var mockLogger = new Mock<ILogger<CreateMotorcycleCommandHandler>>();
        var handler = new CreateMotorcycleCommandHandler(dbContext, validatorMock.Object, mockLogger.Object);
        var command = new CreateMotorcycleCommand
        {
            Identifier = "Yamaha-MT-07",
            Model = "MT-07",
            Year = 2021,
            Plate = "ABC-1234"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsFailure.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task Handle_ShouldTryCreateMotorcycleYearless()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<CreateMotorcycleCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateMotorcycleCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        var mockLogger = new Mock<ILogger<CreateMotorcycleCommandHandler>>();
        var handler = new CreateMotorcycleCommandHandler(fixture.BuildDbContext(Guid.NewGuid().ToString()), validatorMock.Object, mockLogger.Object);
        var command = new CreateMotorcycleCommand
        {
            Identifier = "Yamaha-MT-06",
            Model = "MT-07",
            Plate = "ABC-1234"
        };

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
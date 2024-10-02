using Application.Features.Motorcycles.UpdatePlate;
using Domain.Motorcycles;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Motorcycles.UpdatePlate;

public class UpdateMotorcyclePlateCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldUpdateMotorcyclePlate()
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

        var validatorMock = new Mock<IValidator<UpdateMotorcyclePlateCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateMotorcyclePlateCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var mockLogger = new Mock<ILogger<UpdateMotorcyclePlateCommandHandler>>();
        var handler = new UpdateMotorcyclePlateCommandHandler(dbContext, validatorMock.Object, mockLogger.Object);
        var command = new UpdateMotorcyclePlateCommand
        {
            Identifier = "Yamaha-MT-07",
            Plate = "DEF-5678"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Placa modificada com sucesso");
    }

    [Fact]
    public async Task Handle_ShouldTryUpdateMotorcyclePlateThatNotExists()
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

        var validatorMock = new Mock<IValidator<UpdateMotorcyclePlateCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateMotorcyclePlateCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        var mockLogger = new Mock<ILogger<UpdateMotorcyclePlateCommandHandler>>();
        var handler = new UpdateMotorcyclePlateCommandHandler(dbContext,validatorMock.Object, mockLogger.Object);
        var command = new UpdateMotorcyclePlateCommand
        {
            Identifier = "Yamaha-MT-06",
            Plate = "DEF-5678"
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
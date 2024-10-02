using Application.Abstractions.Storage;
using Application.Features.DeliveryPeople.UpdateDriversLicense;
using Domain.DeliveryPeople;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.DeliveryPeople.UpdateDriversLicense;

public class UpdateDeliveryPersonDriversLicenseCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldUpdateDeliveryPersonDriversLicense()
    {
        // Arrange
        var dbContext = fixture.BuildDbContext(Guid.NewGuid().ToString());

        dbContext
            .DeliveryPeople
            .Add(
                new DeliveryPerson()
                    .SetIdentifier("Person-1")
                    .SetName("Peter Parker")
                    .SetDateOfBirth(new DateTime(2000, 04, 01))
                    .SetEin("94237956000155")
                    .SetDriversLicenseImagem("data:image/png;base64,image.png")
                    .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                    .SetDriversLicenseType(DriversLicenseType.A)
            );

        await dbContext.SaveChangesAsync();

        var validatorMock = new Mock<IValidator<UpdateDeliveryPersonDriversLicenseCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateDeliveryPersonDriversLicenseCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<UpdateDeliveryPersonDriversLicenseCommandHandler>>();
        var handler = new UpdateDeliveryPersonDriversLicenseCommandHandler(dbContext, validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new UpdateDeliveryPersonDriversLicenseCommand
        {
            Identifier = "Person-1",
            DriversLicenseImage = "new.png"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldUpdateDeliveryPersonDriversLicenseThatNotExists()
    {
        // Arrange
        var dbContext = fixture.BuildDbContext(Guid.NewGuid().ToString());

        dbContext
            .DeliveryPeople
            .Add(
                new DeliveryPerson()
                    .SetIdentifier("Person-1")
                    .SetName("Peter Parker")
                    .SetDateOfBirth(new DateTime(2000, 04, 01))
                    .SetEin("94237956000155")
                    .SetDriversLicenseImagem("data:image/png;base64,image.png")
                    .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                    .SetDriversLicenseType(DriversLicenseType.A)
            );

        await dbContext.SaveChangesAsync();

        var validatorMock = new Mock<IValidator<UpdateDeliveryPersonDriversLicenseCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateDeliveryPersonDriversLicenseCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<UpdateDeliveryPersonDriversLicenseCommandHandler>>();
        var handler = new UpdateDeliveryPersonDriversLicenseCommandHandler(dbContext, validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new UpdateDeliveryPersonDriversLicenseCommand
        {
            Identifier = "Person-2",
            DriversLicenseImage = "new.png"
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
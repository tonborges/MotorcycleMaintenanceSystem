using Application.Abstractions.Storage;
using Application.Features.DeliveryPeople.Create;
using Domain.DeliveryPeople;
using FluentAssertions;
using FluentValidation;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.DeliveryPeople.Create;

public class CreateDeliveryPersonCommandHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldCreateDeliveryPerson()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<CreateDeliveryPersonCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateDeliveryPersonCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<CreateDeliveryPersonCommandHandler>>();
        var handler = new CreateDeliveryPersonCommandHandler(fixture.BuildDbContext(Guid.NewGuid().ToString()), validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new CreateDeliveryPersonCommand
        {
            Identifier = "Person-1",
            Name = "Peter Parker",
            DateOfBirth = new DateTime(2000, 04, 01),
            Ein = "94237956000155",
            DriversLicenseImage = "data:image/png;base64,image.png",
            DriversLicenseNumber = Guid.NewGuid().ToString(),
            DriversLicenseType = "A"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldTryCreateDeliveryPersonEqualAnExistingOne()
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
                    .SetDriversLicenseImagem("image.png")
                    .SetDriversLicenseNumber(Guid.NewGuid().ToString())
                    .SetDriversLicenseType(DriversLicenseType.A)
            );

        await dbContext.SaveChangesAsync();
        var validatorMock = new Mock<IValidator<CreateDeliveryPersonCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateDeliveryPersonCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<CreateDeliveryPersonCommandHandler>>();
        var handler = new CreateDeliveryPersonCommandHandler(dbContext, validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new CreateDeliveryPersonCommand
        {
            Identifier = "Person-1",
            Name = "Peter Parker",
            DateOfBirth = new DateTime(2000, 04, 01),
            Ein = "94237956000155",
            DriversLicenseImage = "image.png",
            DriversLicenseNumber = Guid.NewGuid().ToString(),
            DriversLicenseType = "A"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsFailure.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task Handle_ShouldTryCreateDeliveryPersonDriversLicenseWrongCategory()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<CreateDeliveryPersonCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateDeliveryPersonCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<CreateDeliveryPersonCommandHandler>>();
        var handler = new CreateDeliveryPersonCommandHandler(fixture.BuildDbContext(Guid.NewGuid().ToString()), validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new CreateDeliveryPersonCommand
        {
            Identifier = "Person-1",
            Name = "Peter Parker",
            DateOfBirth = new DateTime(2000, 04, 01),
            Ein = "94237956000155",
            DriversLicenseImage = "image.png",
            DriversLicenseNumber = Guid.NewGuid().ToString(),
            DriversLicenseType = "B"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsFailure.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task Handle_ShouldTryCreateDeliveryPersonDriversLicenseless()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<CreateDeliveryPersonCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateDeliveryPersonCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var storageServiceMock = new Mock<IStorageService>();
        storageServiceMock.Setup(s => s.UploadBase64FileAsync(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<CreateDeliveryPersonCommandHandler>>();
        var handler = new CreateDeliveryPersonCommandHandler(fixture.BuildDbContext(Guid.NewGuid().ToString()), validatorMock.Object, mockLogger.Object, storageServiceMock.Object);
        var command = new CreateDeliveryPersonCommand
        {
            Identifier = "Person-1",
            Name = "Peter Parker",
            DateOfBirth = new DateTime(2000, 04, 01),
            Ein = "94237956000155",
        };

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult>();
        result.IsFailure.Should().BeTrue();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Dados inválidos");
    }
}
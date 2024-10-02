using Application.Features.Motorcycles.GetById;
using Domain.Motorcycles;
using FluentAssertions;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Motorcycles.GetById;

public class GetMotorcycleByIdQueryHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldGetMotorcycleByIdExist()
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

        var mockLogger = new Mock<ILogger<GetMotorcycleByIdQueryHandler>>();
        var handler = new GetMotorcycleByIdQueryHandler(dbContext, mockLogger.Object);
        var command = new GetMotorcycleByIdQuery("Yamaha-MT-07");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<GetMotorcycleByIdResponse>>();
        result.IsSuccess.Should().BeTrue();
        result.Data?.Plate.Should().Be("ABC-1234");
    }

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleByIdThatNotExists()
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
                    .SetPlate("ABC-1A34")
            );

        await dbContext.SaveChangesAsync();

        var mockLogger = new Mock<ILogger<GetMotorcycleByIdQueryHandler>>();
        var handler = new GetMotorcycleByIdQueryHandler(dbContext, mockLogger.Object);
        var command = new GetMotorcycleByIdQuery("Yamaha-MT-06");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<GetMotorcycleByIdResponse>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Notifications.Should().NotBeEmpty();
        result.Notifications?.Count().Should().Be(1);
        result.Notifications?.FirstOrDefault()?.Message.Should().Be("Moto não encontrada");
    }
}
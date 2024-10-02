using Application.Features.Motorcycles.Get;
using Domain.Motorcycles;
using FluentAssertions;
using IntegrationTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Results;

namespace IntegrationTest.Motorcycles.Get;

public class GetMotorcycleQueryHandlerTest(ApplicationDbContextFixture fixture) : IClassFixture<ApplicationDbContextFixture>
{
    [Fact]
    public async Task Handle_ShouldGetMotorcycleExists()
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

        var mockLogger = new Mock<ILogger<GetMotorcycleQueryHandler>>();
        var handler = new GetMotorcycleQueryHandler(dbContext, mockLogger.Object);
        var command = new GetMotorcycleQuery("ABC-1234");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<IEnumerable<GetMotorcycleResponse>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data?.Count().Should().Be(1);
        result.Data?.FirstOrDefault()?.Plate.Should().Be("ABC-1234");
    }

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleThatNotExists()
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

        var mockLogger = new Mock<ILogger<GetMotorcycleQueryHandler>>();
        var handler = new GetMotorcycleQueryHandler(dbContext, mockLogger.Object);
        var command = new GetMotorcycleQuery("ABC-1234");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<IEnumerable<GetMotorcycleResponse>>>();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}
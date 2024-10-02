using Application.Abstractions.Data;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntegrationTest.Fixtures;

public class ApplicationDbContextFixture : DbContextFixture<ApplicationDbContext>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly Mock<IPublisher> publisherMock;

    public ApplicationDbContextFixture()
    {
        loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        publisherMock = new Mock<IPublisher>();

        publisherMock
            .Setup(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
    }

    protected override IApplicationDbContext BuildDbContext(DbContextOptions<ApplicationDbContext> options)
    {
        var dbContext = new ApplicationDbContext(options, loggerFactory, publisherMock.Object);
        
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        
        return dbContext;
    }
}
using Application.Abstractions.Data;
using Domain.DeliveryPeople;
using Domain.Motorcycles;
using Domain.Rents;
using Domain.StoredEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Domain;

namespace Infrastructure.Database;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ILoggerFactory loggerFactory,
    IPublisher publisher)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryPerson> DeliveryPeople { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<StoredEvent> StoredEvents { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder
                                 .Model
                                 .GetEntityTypes()
                                 .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(255)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
                           .Entries<Entity>()
                           .Select(entry => entry.Entity)
                           .SelectMany(entity =>
                           {
                               var events = entity.DomainEvents.ToList();
                               entity.ClearDomainEvents();
                               return events;
                           })
                           .ToList();

        var tasks = domainEvents.Select(domainEvent => publisher.Publish(domainEvent));

        await Task.WhenAll(tasks); 
    }
}
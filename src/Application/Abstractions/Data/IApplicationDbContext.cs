using Domain.DeliveryPeople;
using Domain.Motorcycles;
using Domain.Rents;
using Domain.StoredEvents;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryPerson> DeliveryPeople { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<StoredEvent> StoredEvents { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Domain.Rents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.Rents.EntityConfig;

public class RentEntityConfig : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Plan).IsRequired();
        builder.Property(x => x.MotorcycleId).IsRequired();
        builder.Property(x => x.DeliveryPersonId).IsRequired();
        
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : v,
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );
        
        builder.Property(x => x.StartDate).IsRequired().HasConversion(dateTimeConverter);
        builder.Property(x => x.EndDate).IsRequired().HasConversion(dateTimeConverter);
        builder.Property(x => x.ForecastDateEnd).IsRequired().HasConversion(dateTimeConverter);
        builder.Property(x => x.ReturnDate).HasConversion(dateTimeConverter);

        builder.HasOne(x => x.Motorcycle)
               .WithMany(x => x.Rents)
               .HasForeignKey(x => x.MotorcycleId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.DeliveryPerson)
               .WithMany(x => x.Rents)
               .HasForeignKey(x => x.DeliveryPersonId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
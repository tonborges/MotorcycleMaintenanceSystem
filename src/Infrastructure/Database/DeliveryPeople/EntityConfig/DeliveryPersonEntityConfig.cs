using Domain.DeliveryPeople;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.DeliveryPeople.EntityConfig;

public class DeliveryPersonEntityConfig : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
    {
        builder.HasKey(x => x.Identifier);
        
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : v,
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.DateOfBirth).IsRequired().HasConversion(dateTimeConverter);;
        builder.Property(x => x.Ein).IsRequired();
        builder.Property(x => x.DriversLicenseImage).IsRequired();
        builder.Property(x => x.DriversLicenseNumber).IsRequired();
        builder.Property(x => x.DriversLicenseType).IsRequired();

        builder.HasIndex(x => x.Ein)
               .IsUnique();

        builder.HasIndex(x => x.DriversLicenseNumber)
               .IsUnique();

        builder.Property(x => x.DriversLicenseType)
               .HasConversion<string>();

        builder
            .HasMany(x => x.Rents)
            .WithOne(w => w.DeliveryPerson)
            .HasForeignKey(x => x.DeliveryPersonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
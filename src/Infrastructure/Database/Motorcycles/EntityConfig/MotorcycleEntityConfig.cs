using Domain.Motorcycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Motorcycles.EntityConfig;

public class MotorcycleEntityConfig : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.HasKey(x => x.Identifier);

        builder.Property(x => x.Plate).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.Model).IsRequired();

        builder.HasIndex(x => x.Plate)
               .IsUnique();

        builder
            .HasMany(x => x.Rents)
            .WithOne(w => w.Motorcycle)
            .HasForeignKey(x => x.MotorcycleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
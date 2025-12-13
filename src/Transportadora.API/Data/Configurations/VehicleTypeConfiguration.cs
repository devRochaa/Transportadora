using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class VehicleTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(x => x.Plate)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(x => x.Plate)
            .IsUnique();

        builder.Property(x => x.Type)
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
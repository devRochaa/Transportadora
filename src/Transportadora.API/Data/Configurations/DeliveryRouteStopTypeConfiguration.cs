using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class DeliveryRouteStopTypeConfiguration : IEntityTypeConfiguration<DeliveryRouteStop>
{
    public void Configure(EntityTypeBuilder<DeliveryRouteStop> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Latitude)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .IsRequired();

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.HasIndex(x => new { x.RouteId, x.Sequence });

        builder.HasIndex(x => x.Type);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class DeliveryRouteTypeConfiguration : IEntityTypeConfiguration<DeliveryRoute>
{
    public void Configure(EntityTypeBuilder<DeliveryRoute> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.StartedAt);
        builder.Property(x => x.FinishedAt);

        builder.HasIndex(x => new { x.DeliveryId, x.Status });

        builder.HasMany(x => x.Stops)
            .WithOne(x => x.Route)
            .HasForeignKey(x => x.RouteId);

        builder.HasMany(x => x.Positions)
            .WithOne(x => x.Route)
            .HasForeignKey(x => x.RouteId);
    }
}
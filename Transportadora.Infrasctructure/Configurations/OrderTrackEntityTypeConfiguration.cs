using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class OrderTrackEntityTypeConfiguration : IEntityTypeConfiguration<OrderTrackEntity>
{
    public void Configure(EntityTypeBuilder<OrderTrackEntity> builder)
    {
        builder.ToTable("OrderTrack");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.OriginLocation)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(t => t.DestinyLocation)
            .HasMaxLength(300);

        builder.Property(t => t.Distance)
            .HasPrecision(10, 2);

        builder.Property(t => t.HasArrived);

        builder.Property(t => t.Description)
            .HasMaxLength(400);

        builder.HasOne(t => t.Order)
            .WithMany()
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Carrier)
            .WithMany()
            .HasForeignKey(t => t.CarrierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

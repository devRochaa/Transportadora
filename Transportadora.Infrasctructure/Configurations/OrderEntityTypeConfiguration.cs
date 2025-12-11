using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.OrderDate)
            .IsRequired();

        builder.Property(e => e.DeliveryDate);

        builder.Property(e => e.MaxDeliveryDate);

        builder.Property(e => e.TotalAmount)
            .IsRequired();

        builder.Property(t => t.Distance)
            .HasPrecision(10, 2);

        builder.Property(e => e.Freight);

        builder.Property(e => e.WeightCategory)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(e => e.Destinatary)
            .WithMany()
            .HasForeignKey(a => a.DestinataryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.OriginAddress)
            .WithMany()
            .HasForeignKey(a => a.OriginId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DestinyAddress)
            .WithMany()
            .HasForeignKey(a => a.DestinyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

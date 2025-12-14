using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class DeliveryTypeConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EstimatedStartAt)
            .IsRequired();

        builder.Property(x => x.EstimatedEndAt)
            .IsRequired();

        builder.Property(x => x.ActualStartAt);
        builder.Property(x => x.ActualEndAt);

        builder.HasIndex(x => x.Status);

        builder.HasMany(x => x.Routes)
            .WithOne(x => x.Delivery)
            .HasForeignKey(x => x.DeliveryId);

        builder.HasMany(x => x.Events)
            .WithOne(x => x.Delivery)
            .HasForeignKey(x => x.DeliveryId);
    }
}
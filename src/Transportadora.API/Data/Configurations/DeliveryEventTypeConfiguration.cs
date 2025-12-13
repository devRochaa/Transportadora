using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class DeliveryEventTypeConfiguration : IEntityTypeConfiguration<DeliveryEvent>
{
    public void Configure(EntityTypeBuilder<DeliveryEvent> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.DeliveryId, x.CreatedAt });
    }
}
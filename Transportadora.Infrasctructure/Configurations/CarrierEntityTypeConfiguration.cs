using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class CarrierEntityTypeConfiguration : IEntityTypeConfiguration<CarrierEntity>
{
    public void Configure(EntityTypeBuilder<CarrierEntity> builder)
    {
        builder.ToTable("Carrier");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CompanyName)
            .HasMaxLength(60);
            
        builder.Property(e => e.NationalDocument)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(e => e.NationalDocument)
            .IsUnique();

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(15);

        builder.HasIndex(e => e.ContactPhone)
            .IsUnique();
    }
}

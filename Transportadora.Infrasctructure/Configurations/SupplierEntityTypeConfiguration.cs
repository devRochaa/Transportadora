using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<SupplierEntity>
{
    public void Configure(EntityTypeBuilder<SupplierEntity> builder)
    {
        builder.ToTable("Supplier");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.NationalDocument)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(e => e.ContactName)
            .HasMaxLength(100);

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(15);
    }
}

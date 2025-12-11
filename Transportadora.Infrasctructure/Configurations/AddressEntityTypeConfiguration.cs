using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class AddressEntityTypeConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ZipCode)
            .HasMaxLength(9);

        builder.Property(e => e.Country)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(e => e.State)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Neighborhood)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(e => e.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Complement)
            .HasMaxLength(30);

        builder.Property(e => e.ClientId);

        builder.HasOne(e => e.Client)
            .WithMany()
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Supplier)
             .WithMany()
             .HasForeignKey(a => a.SupplierId)
             .OnDelete(DeleteBehavior.Restrict);
    }
}

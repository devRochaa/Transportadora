using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.API.Data.Entities;

namespace Transportadora.API.Data.Configurations;

public class DriverTypeConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Document)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.Document)
            .IsUnique();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .IsRequired();
    }
}

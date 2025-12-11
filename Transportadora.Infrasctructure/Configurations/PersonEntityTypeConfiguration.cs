using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transportadora.Domain.Entities;

namespace Transportadora.Infrasctructure.Configurations;

internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.ToTable("Person");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Fullname)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.NationalDocument)
               .HasMaxLength(14)
               .IsRequired();

        builder.HasIndex(c => c.NationalDocument)
               .IsUnique(); 

        builder.Property(c => c.Phone)
               .HasMaxLength(50);

        builder.HasIndex(c => c.Phone)
              .IsUnique();

        builder.Property(c => c.BirthDate)
               .IsRequired();
    }
}

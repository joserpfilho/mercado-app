using MercadoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoApp.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
        builder.Property(i => i.Unit).HasConversion<string>();

        builder.HasOne(i => i.Group)
            .WithMany(g => g.Items)
            .HasForeignKey(i => i.GroupId);
    }
}
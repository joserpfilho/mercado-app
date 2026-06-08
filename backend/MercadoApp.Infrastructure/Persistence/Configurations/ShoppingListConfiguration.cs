using MercadoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoApp.Infrastructure.Persistence.Configurations;

public class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Name).IsRequired().HasMaxLength(100);
        builder.Property(sl => sl.Status).HasConversion<string>();

        builder.HasOne(sl => sl.Group)
            .WithMany(g => g.ShoppingLists)
            .HasForeignKey(sl => sl.GroupId);
    }
}
using MercadoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MercadoApp.Infrastructure.Persistence.Configurations;

public class ListItemConfiguration : IEntityTypeConfiguration<ListItem>
{
    public void Configure(EntityTypeBuilder<ListItem> builder)
    {
        builder.HasKey(li => li.Id);
        builder.Property(li => li.Quantity).HasPrecision(10, 3);

        builder.HasOne(li => li.ShoppingList)
            .WithMany(sl => sl.ListItems)
            .HasForeignKey(li => li.ShoppingListId);

        builder.HasOne(li => li.Item)
            .WithMany(i => i.ListItems)
            .HasForeignKey(li => li.ItemId);

        builder.HasOne(li => li.Department)
            .WithMany(d => d.ListItems)
            .HasForeignKey(li => li.DepartmentId);
    }
}
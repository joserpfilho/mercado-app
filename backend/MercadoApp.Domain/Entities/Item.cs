using MercadoApp.Domain.Enums;

namespace MercadoApp.Domain.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemUnit Unit { get; set; } = ItemUnit.Un;
    public Guid GroupId { get; set; }

    public Group Group { get; set; } = null!;
    public ICollection<ListItem> ListItems { get; set; } = [];
}
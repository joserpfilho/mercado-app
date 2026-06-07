using MercadoApp.Domain.Enums;

namespace MercadoApp.Domain.Entities;

public class ShoppingList
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ListStatus Status { get; set; } = ListStatus.Active;
    public Guid GroupId { get; set; }

    public Group Group { get; set; } = null!;
    public ICollection<ListItem> ListItems { get; set; } = [];
}
namespace MercadoApp.Domain.Entities;

public class ListItem
{
    public Guid Id { get; set; }
    public decimal Quantity { get; set; } = 1;
    public bool IsChecked { get; set; } = false;

    public Guid ShoppingListId { get; set; }
    public Guid ItemId { get; set; }
    public Guid DepartmentId { get; set; }

    public ShoppingList ShoppingList { get; set; } = null!;
    public Item Item { get; set; } = null!;
    public Department Department { get; set; } = null!;
}
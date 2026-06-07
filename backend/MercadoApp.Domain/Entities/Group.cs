namespace MercadoApp.Domain.Entities;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMember> Members { get; set; } = [];
    public ICollection<Department> Departments { get; set; } = [];
    public ICollection<Item> Items { get; set; } = [];
    public ICollection<ShoppingList> ShoppingLists { get; set; } = [];
}
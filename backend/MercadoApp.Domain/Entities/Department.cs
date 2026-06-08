using MercadoApp.Domain.Common;

namespace MercadoApp.Domain.Entities;

public class Department : ISoftDeletable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public Guid GroupId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Group Group { get; set; } = null!;
    public ICollection<ListItem> ListItems { get; set; } = [];
}
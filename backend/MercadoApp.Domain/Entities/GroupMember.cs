using MercadoApp.Domain.Enums;

namespace MercadoApp.Domain.Entities;

public class GroupMember
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public GroupRole Role { get; set; } = GroupRole.Member;

    public User User { get; set; } = null!;
    public Group Group { get; set; } = null!;
}
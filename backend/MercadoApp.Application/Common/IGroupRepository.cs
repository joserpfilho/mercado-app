using MercadoApp.Domain.Entities;

namespace MercadoApp.Application.Common;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id);
    Task<Group?> GetByIdWithMembersAsync(Guid id);
    Task<List<Group>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Group group);
    Task AddMemberAsync(GroupMember member);
    Task SaveChangesAsync();
}
using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class GroupRepository(AppDbContext context) : IGroupRepository
{
    public async Task<Group?> GetByIdAsync(Guid id) =>
        await context.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == id);

    public async Task<List<Group>> GetByUserIdAsync(Guid userId) =>
        await context.Groups
            .Where(g => g.Members.Any(m => m.UserId == userId))
            .ToListAsync();

    public async Task AddAsync(Group group) =>
        await context.Groups.AddAsync(group);

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}
using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<List<Item>> GetByGroupIdAsync(Guid groupId) =>
        await context.Items
            .Where(i => i.GroupId == groupId)
            .ToListAsync();

    public async Task<Item?> GetByIdAsync(Guid id) =>
        await context.Items.FindAsync(id);

    public async Task AddAsync(Item item) =>
        await context.Items.AddAsync(item);

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}
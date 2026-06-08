using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class ShoppingListRepository(AppDbContext context) : IShoppingListRepository
{
    public async Task<ShoppingList?> GetByIdWithItemsAsync(Guid id) =>
        await context.ShoppingLists
            .Include(sl => sl.ListItems)
                .ThenInclude(li => li.Item)
            .Include(sl => sl.ListItems)
                .ThenInclude(li => li.Department)
            .FirstOrDefaultAsync(sl => sl.Id == id);

    public async Task<List<ShoppingList>> GetByGroupIdAsync(Guid groupId, ListStatus? status = null) =>
        await context.ShoppingLists
            .Include(sl => sl.ListItems)
            .Where(sl => sl.GroupId == groupId && (status == null || sl.Status == status))
            .OrderByDescending(sl => sl.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(ShoppingList list) =>
        await context.ShoppingLists.AddAsync(list);

    public async Task AddListItemAsync(ListItem listItem) =>
        await context.ListItems.AddAsync(listItem);

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.Common;

public interface IShoppingListRepository
{
    Task<ShoppingList?> GetByIdWithItemsAsync(Guid id);
    Task<List<ShoppingList>> GetByGroupIdAsync(Guid groupId, ListStatus? status = null);
    Task AddAsync(ShoppingList list);
    Task AddListItemAsync(ListItem listItem);
    Task SaveChangesAsync();
}
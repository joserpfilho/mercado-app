using MercadoApp.Domain.Entities;

namespace MercadoApp.Application.Common;

public interface IItemRepository
{
    Task<List<Item>> GetByGroupIdAsync(Guid groupId);
    Task<Item?> GetByIdAsync(Guid id);
    Task AddAsync(Item item);
    Task SaveChangesAsync();
}
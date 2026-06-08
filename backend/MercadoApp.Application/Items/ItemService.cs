using MercadoApp.Application.Common;
using MercadoApp.Application.Items.DTOs;
using MercadoApp.Domain.Entities;

namespace MercadoApp.Application.Items;

public class ItemService(IItemRepository itemRepository, IGroupRepository groupRepository)
{
    public async Task<Result<ItemResponse>> CreateAsync(CreateItemRequest request, Guid groupId)
    {
        var group = await groupRepository.GetByIdAsync(groupId);
        if (group is null)
            return Result<ItemResponse>.Failure("Grupo não encontrado.");

        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Unit = request.Unit,
            GroupId = groupId
        };

        await itemRepository.AddAsync(item);
        await itemRepository.SaveChangesAsync();

        return Result<ItemResponse>.Success(new ItemResponse(item.Id, item.Name, item.Unit));
    }

    public async Task<Result<bool>> DeleteAsync(Guid itemId)
    {
        var item = await itemRepository.GetByIdAsync(itemId);
        if (item is null)
            return Result<bool>.Failure("Item não encontrado.");

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        await itemRepository.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<ItemResponse>>> GetByGroupAsync(Guid groupId)
    {
        var items = await itemRepository.GetByGroupIdAsync(groupId);
        var response = items
            .Select(i => new ItemResponse(i.Id, i.Name, i.Unit))
            .ToList();

        return Result<List<ItemResponse>>.Success(response);
    }
}
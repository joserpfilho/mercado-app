using MercadoApp.Application.Common;
using MercadoApp.Application.ShoppingLists.DTOs;
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.ShoppingLists;

public class ShoppingListService(
    IShoppingListRepository shoppingListRepository,
    IGroupRepository groupRepository,
    IItemRepository itemRepository,
    IDepartmentRepository departmentRepository)
{
    public async Task<Result<ShoppingListSummaryResponse>> CreateAsync(
        CreateShoppingListRequest request, Guid groupId)
    {
        var group = await groupRepository.GetByIdAsync(groupId);
        if (group is null)
            return Result<ShoppingListSummaryResponse>.Failure("Grupo não encontrado.");

        var list = new ShoppingList
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            GroupId = groupId
        };

        await shoppingListRepository.AddAsync(list);
        await shoppingListRepository.SaveChangesAsync();

        return Result<ShoppingListSummaryResponse>.Success(
            new ShoppingListSummaryResponse(list.Id, list.Name, list.CreatedAt, list.Status, 0, 0));
    }

    public async Task<Result<List<ShoppingListSummaryResponse>>> GetByGroupAsync(Guid groupId, ListStatus? status = null)
    {
        var lists = await shoppingListRepository.GetByGroupIdAsync(groupId, status);
        var response = lists.Select(l => new ShoppingListSummaryResponse(
            l.Id, l.Name, l.CreatedAt, l.Status,
            l.ListItems.Count,
            l.ListItems.Count(i => i.IsChecked))).ToList();

        return Result<List<ShoppingListSummaryResponse>>.Success(response);
    }

    public async Task<Result<ShoppingListResponse>> GetByIdAsync(Guid id)
    {
        var list = await shoppingListRepository.GetByIdWithItemsAsync(id);
        if (list is null)
            return Result<ShoppingListResponse>.Failure("Lista não encontrada.");

        var response = MapToResponse(list);
        return Result<ShoppingListResponse>.Success(response);
    }

    public async Task<Result<ShoppingListResponse>> AddItemAsync(Guid listId, AddListItemRequest request)
    {
        var list = await shoppingListRepository.GetByIdWithItemsAsync(listId);
        if (list is null)
            return Result<ShoppingListResponse>.Failure("Lista não encontrada.");

        var item = await itemRepository.GetByIdAsync(request.ItemId);
        if (item is null)
            return Result<ShoppingListResponse>.Failure("Item não encontrado.");

        if (item.GroupId != list.GroupId)
            return Result<ShoppingListResponse>.Failure("Item não pertence ao grupo desta lista.");

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId);
        if (department is null)
            return Result<ShoppingListResponse>.Failure("Departamento não encontrado.");

        if (department.GroupId != list.GroupId)
            return Result<ShoppingListResponse>.Failure("Departamento não pertence ao grupo desta lista.");

        var listItem = new ListItem
        {
            Id = Guid.NewGuid(),
            ShoppingListId = listId,
            ItemId = request.ItemId,
            DepartmentId = request.DepartmentId,
            Quantity = request.Quantity
        };

        await shoppingListRepository.AddListItemAsync(listItem);
        await shoppingListRepository.SaveChangesAsync();

        var updatedList = await shoppingListRepository.GetByIdWithItemsAsync(listId);
        return Result<ShoppingListResponse>.Success(MapToResponse(updatedList!));
    }

    public async Task<Result<ShoppingListResponse>> UpdateItemAsync(
        Guid listId, Guid listItemId, UpdateListItemRequest request)
    {
        var list = await shoppingListRepository.GetByIdWithItemsAsync(listId);
        if (list is null)
            return Result<ShoppingListResponse>.Failure("Lista não encontrada.");

        var listItem = list.ListItems.FirstOrDefault(i => i.Id == listItemId);
        if (listItem is null)
            return Result<ShoppingListResponse>.Failure("Item não encontrado na lista.");

        if (request.IsChecked.HasValue)
            listItem.IsChecked = request.IsChecked.Value;

        if (request.Quantity.HasValue)
            listItem.Quantity = request.Quantity.Value;

        await shoppingListRepository.SaveChangesAsync();
        return Result<ShoppingListResponse>.Success(MapToResponse(list));
    }

    public async Task<Result<ShoppingListSummaryResponse>> ArchiveAsync(Guid listId)
    {
        var list = await shoppingListRepository.GetByIdWithItemsAsync(listId);
        if (list is null)
            return Result<ShoppingListSummaryResponse>.Failure("Lista não encontrada.");

        list.Status = ListStatus.Archived;
        await shoppingListRepository.SaveChangesAsync();

        return Result<ShoppingListSummaryResponse>.Success(
            new ShoppingListSummaryResponse(list.Id, list.Name, list.CreatedAt, list.Status,
                list.ListItems.Count, list.ListItems.Count(i => i.IsChecked)));
    }

    public async Task<Result<ShoppingListSummaryResponse>> DeleteAsync(Guid listId)
    {
        var list = await shoppingListRepository.GetByIdWithItemsAsync(listId);
        if (list is null)
            return Result<ShoppingListSummaryResponse>.Failure("Lista não encontrada.");

        list.Status = ListStatus.Deleted;
        await shoppingListRepository.SaveChangesAsync();

        return Result<ShoppingListSummaryResponse>.Success(
            new ShoppingListSummaryResponse(list.Id, list.Name, list.CreatedAt, list.Status,
                list.ListItems.Count, list.ListItems.Count(i => i.IsChecked)));
    }

    private static ShoppingListResponse MapToResponse(ShoppingList list) =>
        new(list.Id, list.Name, list.CreatedAt, list.Status,
            list.ListItems.Select(li => new ListItemResponse(
                li.Id,
                li.ItemId,
                li.Item?.Name ?? string.Empty,
                li.Item?.Unit ?? ItemUnit.Un,
                li.Quantity,
                li.IsChecked,
                li.DepartmentId,
                li.Department?.Name ?? string.Empty,
                li.Department?.Icon ?? string.Empty)).ToList());
}
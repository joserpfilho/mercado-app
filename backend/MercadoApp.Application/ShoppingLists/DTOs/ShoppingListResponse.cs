using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.ShoppingLists.DTOs;

public record ShoppingListResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    ListStatus Status,
    List<ListItemResponse> Items);
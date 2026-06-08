using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.ShoppingLists.DTOs;

public record ShoppingListSummaryResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    ListStatus Status,
    int TotalItems,
    int CheckedItems);
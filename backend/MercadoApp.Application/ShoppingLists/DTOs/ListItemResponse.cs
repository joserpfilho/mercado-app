using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.ShoppingLists.DTOs;

public record ListItemResponse(
    Guid Id,
    Guid ItemId,
    string ItemName,
    ItemUnit Unit,
    decimal Quantity,
    bool IsChecked,
    Guid DepartmentId,
    string DepartmentName,
    string DepartmentIcon);
namespace MercadoApp.Application.ShoppingLists.DTOs;

public record AddListItemRequest(Guid ItemId, Guid DepartmentId, decimal Quantity);
namespace MercadoApp.Application.ShoppingLists.DTOs;

public record UpdateListItemRequest(bool? IsChecked, decimal? Quantity);
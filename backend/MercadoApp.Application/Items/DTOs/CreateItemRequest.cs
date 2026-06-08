using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.Items.DTOs;

public record CreateItemRequest(string Name, ItemUnit Unit);
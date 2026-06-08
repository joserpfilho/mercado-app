using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.Items.DTOs;

public record ItemResponse(Guid Id, string Name, ItemUnit Unit);
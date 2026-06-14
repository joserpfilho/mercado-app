namespace MercadoApp.Application.Groups.DTOs;

public record GroupMemberResponse(Guid UserId, string Name, string Email, string Role);
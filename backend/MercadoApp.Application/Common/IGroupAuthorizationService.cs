namespace MercadoApp.Application.Common;

public interface IGroupAuthorizationService
{
    Task<bool> IsMemberAsync(Guid groupId, Guid userId);
}
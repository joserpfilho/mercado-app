using System.Security.Claims;
using MercadoApp.Application.Common;

namespace MercadoApp.API.Filters;

public class ListMembershipFilter(
    IShoppingListRepository listRepository,
    IGroupAuthorizationService authService) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var routeValue = context.HttpContext.Request.RouteValues["id"]?.ToString();

        if (!Guid.TryParse(routeValue, out var listId))
            return Results.BadRequest(new { error = "id inválido." });

        var list = await listRepository.GetByIdWithItemsAsync(listId);
        if (list is null)
            return Results.NotFound(new { error = "Lista não encontrada." });

        var user = context.HttpContext.User;
        var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var isMember = await authService.IsMemberAsync(list.GroupId, userId);
        if (!isMember)
            return Results.Forbid();

        return await next(context);
    }
}
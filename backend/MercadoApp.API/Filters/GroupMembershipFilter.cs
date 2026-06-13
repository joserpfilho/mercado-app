using System.Security.Claims;
using MercadoApp.Application.Common;

namespace MercadoApp.API.Filters;

public class GroupMembershipFilter(IGroupAuthorizationService authService) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var routeValue = context.HttpContext.Request.RouteValues["groupId"]?.ToString();

        if (!Guid.TryParse(routeValue, out var groupId))
            return Results.BadRequest(new { error = "groupId inválido." });

        var user = context.HttpContext.User;
        var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var isMember = await authService.IsMemberAsync(groupId, userId);
        if (!isMember)
            return Results.Forbid();

        return await next(context);
    }
}
using System.Security.Claims;
using Carter;
using FluentValidation;
using MercadoApp.Application.Groups;
using MercadoApp.Application.Groups.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MercadoApp.API.Modules;

public class GroupModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/groups")
            .WithTags("Groups")
            .RequireAuthorization();

        group.MapGet("/", async (
            ClaimsPrincipal user,
            [FromServices] GroupService groupService) =>
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await groupService.GetMyGroupsAsync(userId);
            return Results.Ok(result.Value);
        });

        group.MapPost("/", async (
            [FromBody] CreateGroupRequest request,
            ClaimsPrincipal user,
            [FromServices] GroupService groupService,
            [FromServices] IValidator<CreateGroupRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await groupService.CreateAsync(request, userId);

            return result.IsSuccess
                ? Results.Created($"/groups/{result.Value!.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        });
    }
}
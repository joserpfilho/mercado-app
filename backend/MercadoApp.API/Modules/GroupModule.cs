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

        group.MapGet("/{groupId}/members", async (
            Guid groupId,
            [FromServices] GroupService groupService) =>
        {
            var result = await groupService.GetMembersAsync(groupId);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .AddEndpointFilter<MercadoApp.API.Filters.GroupMembershipFilter>();

        group.MapPost("/{groupId}/members", async (
            Guid groupId,
            [FromBody] AddMemberRequest request,
            [FromServices] GroupService groupService,
            [FromServices] IValidator<AddMemberRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await groupService.AddMemberAsync(groupId, request);
            return result.IsSuccess
                ? Results.Created($"/groups/{groupId}/members/{result.Value!.UserId}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .AddEndpointFilter<MercadoApp.API.Filters.GroupMembershipFilter>();
    }
}
using Carter;
using FluentValidation;
using MercadoApp.Application.Items;
using MercadoApp.Application.Items.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MercadoApp.API.Modules;

public class ItemModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/groups/{groupId}/items")
            .WithTags("Items")
            .RequireAuthorization();

        group.MapGet("/", async (
            Guid groupId,
            [FromServices] ItemService itemService) =>
        {
            var result = await itemService.GetByGroupAsync(groupId);
            return Results.Ok(result.Value);
        });

        group.MapPost("/", async (
            Guid groupId,
            [FromBody] CreateItemRequest request,
            [FromServices] ItemService itemService,
            [FromServices] IValidator<CreateItemRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await itemService.CreateAsync(request, groupId);
            return result.IsSuccess
                ? Results.Created($"/groups/{groupId}/items/{result.Value!.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        });

        group.MapDelete("/{itemId}", async (
            Guid groupId,
            Guid itemId,
            [FromServices] ItemService itemService) =>
        {
            var result = await itemService.DeleteAsync(itemId);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Error });
        });
    }
}
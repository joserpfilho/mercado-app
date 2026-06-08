using Carter;
using FluentValidation;
using MercadoApp.Application.ShoppingLists;
using MercadoApp.Application.ShoppingLists.DTOs;
using MercadoApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MercadoApp.API.Modules;

public class ShoppingListModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var lists = app.MapGroup("/groups/{groupId}/lists")
            .WithTags("ShoppingLists")
            .RequireAuthorization();

        lists.MapGet("/", async (
            Guid groupId,
            [FromServices] ShoppingListService service,
            [FromQuery] ListStatus? status = null) =>
        {
            var result = await service.GetByGroupAsync(groupId, status);
            return Results.Ok(result.Value);
        });

        lists.MapPost("/", async (
            Guid groupId,
            [FromBody] CreateShoppingListRequest request,
            [FromServices] ShoppingListService service,
            [FromServices] IValidator<CreateShoppingListRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await service.CreateAsync(request, groupId);
            return result.IsSuccess
                ? Results.Created($"/groups/{groupId}/lists/{result.Value!.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        });

        var list = app.MapGroup("/lists")
            .WithTags("ShoppingLists")
            .RequireAuthorization();

        list.MapGet("/{id}", async (
            Guid id,
            [FromServices] ShoppingListService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        });

        list.MapPost("/{id}/items", async (
            Guid id,
            [FromBody] AddListItemRequest request,
            [FromServices] ShoppingListService service,
            [FromServices] IValidator<AddListItemRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await service.AddItemAsync(id, request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        });

        list.MapPatch("/{id}/items/{listItemId}", async (
            Guid id,
            Guid listItemId,
            [FromBody] UpdateListItemRequest request,
            [FromServices] ShoppingListService service) =>
        {
            var result = await service.UpdateItemAsync(id, listItemId, request);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        });

        list.MapPatch("/{id}/archive", async (
            Guid id,
            [FromServices] ShoppingListService service) =>
        {
            var result = await service.ArchiveAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        });
        
        list.MapDelete("/{id}/delete", async (
            Guid id,
            [FromServices] ShoppingListService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        });
    }
}
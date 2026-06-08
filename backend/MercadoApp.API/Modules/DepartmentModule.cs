using Carter;
using FluentValidation;
using MercadoApp.Application.Departments;
using MercadoApp.Application.Departments.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MercadoApp.API.Modules;

public class DepartmentModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/groups/{groupId}/departments")
            .WithTags("Departments")
            .RequireAuthorization();

        group.MapGet("/", async (
            Guid groupId,
            [FromServices] DepartmentService departmentService) =>
        {
            var result = await departmentService.GetByGroupAsync(groupId);
            return Results.Ok(result.Value);
        });

        group.MapPost("/", async (
            Guid groupId,
            [FromBody] CreateDepartmentRequest request,
            [FromServices] DepartmentService departmentService,
            [FromServices] IValidator<CreateDepartmentRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await departmentService.CreateAsync(request, groupId);

            return result.IsSuccess
                ? Results.Created($"/groups/{groupId}/departments/{result.Value!.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        });
    }
}
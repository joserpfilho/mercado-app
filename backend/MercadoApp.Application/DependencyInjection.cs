using FluentValidation;
using MercadoApp.Application.Auth;
using MercadoApp.Application.Departments;
using MercadoApp.Application.Groups;
using Microsoft.Extensions.DependencyInjection;

namespace MercadoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<GroupService>();
        services.AddScoped<DepartmentService>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}
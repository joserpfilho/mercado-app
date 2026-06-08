using FluentValidation;
using MercadoApp.Application.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace MercadoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}
using Berrilan.Claims.Api.ExceptionHandlers;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Berrilan.Claims.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ItemNotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
using Berrilan.Claims.WebApi.ExceptionHandlers;

namespace Berrilan.Claims.WebApi;

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
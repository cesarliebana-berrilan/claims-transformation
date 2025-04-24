using Berrilan.Claims.Blazor8.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Berrilan.Claims.Blazor8;

internal static class DependencyInjection
{
    internal static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiConfiguration>(configuration.GetSection("Api"));
    }

    internal static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CustomAuthenticationMessageHandler>();
        services.AddHttpClient<ApiService>(client =>
        {
            ApiConfiguration? apiConfig = configuration.GetSection("Api").Get<ApiConfiguration>();
            client.BaseAddress = new Uri(apiConfig!.Url);
        }).AddHttpMessageHandler<CustomAuthenticationMessageHandler>();
    }

    internal static void AddSecurityLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOidcAuthentication(options =>
        {
            configuration.Bind("Oidc", options.ProviderOptions);
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", configuration["Oidc:Audience"]!);
            options.ProviderOptions.DefaultScopes.Clear();
            options.ProviderOptions.DefaultScopes.Add("openid");
            options.ProviderOptions.DefaultScopes.Add("profile");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.DefaultScopes.Add("offline_access");            
        }).AddAccountClaimsPrincipalFactory<CustomUserFactory>();

    }
}

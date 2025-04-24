using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace Berrilan.Claims.Blazor8.Services;

public class CustomAuthenticationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthenticationMessageHandler(IAccessTokenProvider provider, NavigationManager nav, IOptions<ApiConfiguration> configuration) : base(provider, nav)
    {
        ConfigureHandler([ configuration.Value.Url ]);            
    }
}

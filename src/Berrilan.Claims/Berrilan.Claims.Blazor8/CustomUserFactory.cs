using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Berrilan.Claims.Blazor8.Services;
using static System.Net.WebRequestMethods;

namespace Berrilan.Claims.Blazor8;

public class CustomUserFactory(IAccessTokenProviderAccessor accessor, IServiceProvider serviceProvider) : AccountClaimsPrincipalFactory<RemoteUserAccount>(accessor)
{

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        ClaimsPrincipal user = await base.CreateUserAsync(account, options);
        if (user.Identity?.IsAuthenticated == true)
        {
            ApiService apiService = serviceProvider.GetRequiredService<ApiService>();
            GetMeResponse response = await apiService.GetMe();
            ClaimsIdentity identity = (ClaimsIdentity)user.Identity;
            identity.AddClaim(new Claim("ExternalInfo", JsonSerializer.Serialize(response)));
        }
        return user;
    }
}

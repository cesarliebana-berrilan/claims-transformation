using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Berrilan.Claims.Blazor8;

public class CustomUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    private readonly HttpClient _http;

    public CustomUserFactory(IAccessTokenProviderAccessor accessor, HttpClient http) : base(accessor)
    {
        _http = http;
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        // 1) Delega en la fábrica por defecto para crear el principal
        var user = await base.CreateUserAsync(account, options);

        if (user.Identity?.IsAuthenticated == true)
        {
            //// 2) Pide el access token
            //var tokenResult = await Provider.RequestAccessToken();
            //if (tokenResult.TryGetToken(out var token))
            //{
            //    _http.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", token.Value);

            //    // 3) Llama a tu API externo y añade un claim
            //    var info = await _http.GetFromJsonAsync<ExternalInfo>("https://api.externo/info");
            //    var identity = (ClaimsIdentity)user.Identity;
            //    identity.AddClaim(new Claim("ExternalInfo",
            //        JsonSerializer.Serialize(info)));
            //}
        }
        return user;
    }
}

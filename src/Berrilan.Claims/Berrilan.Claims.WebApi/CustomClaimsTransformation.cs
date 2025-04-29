using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Berrilan.Claims.WebApi;

public class CustomClaimsTransformation(IHttpContextAccessor httpContext, IUserContext userContext) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (httpContext.HttpContext?.Request.Headers.TryGetValue("Authorization", out var header) == true)
        {
            string token = header.ToString().Substring(7);

            UserInfo userInfo = await userContext.GetUserInfo(token)
                ?? throw new CredentialNotValidException($"User {principal.Identity?.Name} not authorized");

            AddCustomClaims(principal, userInfo);
        }

        return principal;
    }

    private static void AddCustomClaims(ClaimsPrincipal principal, UserInfo userInfo)
    {
        ClaimsIdentity claimsIdentity = new();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role.ToString(), userInfo.Role.ToString()));
        if (userInfo.IsRoot) claimsIdentity.AddClaim(new Claim(ClaimTypes.Role.ToString(), "root"));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.User), userInfo.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.Customer), userInfo.CustomerId.ToString()));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.License), userInfo.License.ToString()));
        principal.AddIdentity(claimsIdentity);
    }
}



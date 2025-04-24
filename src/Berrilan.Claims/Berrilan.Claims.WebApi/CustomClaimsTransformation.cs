using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Berrilan.Claims.Core.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Berrilan.Claims.WebApi;

public class CustomClaimsTransformation(IMemoryCache cache, AppDbContext dbContext) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        string email = principal.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value 
            ?? throw new CredentialNotValidException($"User {principal.Identity?.Name} not authorized");

        User user = await GetUser(email)
            ?? throw new CredentialNotValidException($"User {principal.Identity?.Name} not authorized");

        ClaimsIdentity claimsIdentity = new();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role.ToString(), user.Role.ToString()));
        if (user.IsRoot) claimsIdentity.AddClaim(new Claim(ClaimTypes.Role.ToString(), "root"));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.User), user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.Customer), user.CustomerId.ToString()));
        claimsIdentity.AddClaim(new Claim(nameof(CustomClaimTypes.License), user.License.ToString()));
        principal.AddIdentity(claimsIdentity);
        
        return principal;
    }

    private async Task<User?> GetUser(string eMail)
    {
        User? user = await cache.GetOrCreateAsync("principal.Identity.Name", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return dbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == eMail.ToLower());
        });

        return user;
    }
}

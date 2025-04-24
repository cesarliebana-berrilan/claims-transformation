using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Berrilan.Claims.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace Berrilan.Claims.WebApi;

public class UserContext(IHttpContextAccessor _httpContextAccessor, IMemoryCache cache, AppDbContext dbContext) : IUserContext
{
    private const string Message = "User context is unavailable";

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User
        .Identity?
        .IsAuthenticated
        ?? false;

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(x => x.Type == nameof(CustomClaimTypes.User))?.Value 
        ?? throw new CredentialNotValidException(Message));

    public Guid CustomerId => Guid.Parse(_httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(x => x.Type == nameof(CustomClaimTypes.Customer))?.Value
        ?? throw new CredentialNotValidException(Message));

    public string LicenseId => _httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(x => x.Type == nameof(CustomClaimTypes.License))?.Value
        ?? throw new CredentialNotValidException(Message);

    public async Task<UserInfo?> GetUserInfo(string accessToken)
    {
        UserInfo? userInfo = await cache.GetOrCreateAsync(accessToken, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            GetUserProfileResponse userProfile = await httpClient.GetFromJsonAsync<GetUserProfileResponse>("https://api.userprofile.autodesk.com/userinfo")
                ?? throw new CredentialNotValidException($"User not authorized");

            User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == userProfile.Email.ToLower())
                ?? throw new CredentialNotValidException($"User not authorized");

            return new UserInfo(user.Id, user.CustomerId, user.License, user.Role, user.IsRoot, user.Email, userProfile.Name, userProfile.Picture);
        });

        return userInfo;
    }
}

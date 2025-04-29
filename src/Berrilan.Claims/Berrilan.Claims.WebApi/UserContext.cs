using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Berrilan.Claims.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Berrilan.Claims.WebApi;

public class UserContext(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, AppDbContext dbContext) : IUserContext
{
    private const string Message = "User context is unavailable";

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User
        .Identity?
        .IsAuthenticated
        ?? false;

    public Guid UserId => Guid.Parse(httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(x => x.Type == nameof(CustomClaimTypes.User))?.Value 
        ?? throw new CredentialNotValidException(Message));

    public Guid CustomerId => Guid.Parse(httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(x => x.Type == nameof(CustomClaimTypes.Customer))?.Value
        ?? throw new CredentialNotValidException(Message));

    public string LicenseId => httpContextAccessor.HttpContext?.User
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

public record GetUserProfileResponse(
    [property: JsonPropertyName("sub")] string Sub,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("given_name")] string GivenName,
    [property: JsonPropertyName("family_name")] string FamilyName,
    [property: JsonPropertyName("preferred_username")] string PreferredUsername,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("email_verified")] bool? EmailVerified,
    [property: JsonPropertyName("profile")] string Profile,
    [property: JsonPropertyName("picture")] string Picture,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("updated_at")] int? UpdatedAt,
    [property: JsonPropertyName("is_2fa_enabled")] bool? Is2faEnabled,
    [property: JsonPropertyName("country_code")] string CountryCode,
    [property: JsonPropertyName("phone_number")] string PhoneNumber,
    [property: JsonPropertyName("phone_number_verified")] bool? PhoneNumberVerified,
    [property: JsonPropertyName("ldap_enabled")] bool? LdapEnabled,
    [property: JsonPropertyName("ldap_domain")] string LdapDomain,
    [property: JsonPropertyName("job_title")] string JobTitle,
    [property: JsonPropertyName("industry")] string Industry,
    [property: JsonPropertyName("industry_code")] string IndustryCode,
    [property: JsonPropertyName("about_me")] string AboutMe,
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("company")] string Company,
    [property: JsonPropertyName("created_date")] DateTime? CreatedDate,
    [property: JsonPropertyName("last_login_date")] DateTime? LastLoginDate,
    [property: JsonPropertyName("eidm_guid")] string EidmGuid,
    [property: JsonPropertyName("opt_in")] bool? OptIn
);

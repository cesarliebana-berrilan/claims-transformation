using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;

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

    //private async Task<UserInfo?> GetUser(string accessToken, string? name)
    //{
    //    UserInfo? userInfo = await cache.GetOrCreateAsync(accessToken, async entry =>
    //    {
    //        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
    //        HttpClient httpClient = new();
    //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    //        GetUserProfileResponse userProfile = await httpClient.GetFromJsonAsync<GetUserProfileResponse>("https://api.userprofile.autodesk.com/userinfo")
    //            ?? throw new CredentialNotValidException($"User {name} not authorized");
            
    //        User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == userProfile.Email.ToLower())
    //            ?? throw new CredentialNotValidException($"User {name} not authorized");

    //        return new UserInfo(user.Id, user.CustomerId, user.License, user.Role, user.IsRoot, user.Email, userProfile.Name, userProfile.Picture);
    //    });

    //    return userInfo;
    //}
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

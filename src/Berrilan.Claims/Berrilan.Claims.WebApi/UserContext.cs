using Berrilan.Claims.Core;
using Berrilan.Claims.Core.Exceptions;

namespace Berrilan.Claims.WebApi;

public class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
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
}

using Berrilan.Claims.Core.Domain;

namespace Berrilan.Claims.Core;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    Guid CustomerId { get; }

    string LicenseId { get; }

    Task<UserInfo?> GetUserInfo(string accessToken);
}

public enum CustomClaimTypes
{
    User,
    Customer,
    License
}

public record UserInfo(Guid Id, Guid CustomerId, License License, Role Role, bool IsRoot, string Email, string Name, string Picture);

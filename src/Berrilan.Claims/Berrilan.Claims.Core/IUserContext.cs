namespace Berrilan.Claims.Core;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    Guid CustomerId { get; }

    string LicenseId { get; }
}

public enum CustomClaimTypes
{
    User,
    Customer,
    License
}

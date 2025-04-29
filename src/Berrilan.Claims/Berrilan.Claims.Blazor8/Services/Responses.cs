namespace Berrilan.Claims.Blazor8.Services;

public record struct GetMeResponse(Guid Id, Guid CustomerId, License License, Role Role, bool IsRoot, string Email, string Name, string Picture);

public enum Role
{
    Admin,
    User,
    Guest
}

public enum License
{
    Free,
    Enterprise
}
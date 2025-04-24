namespace Berrilan.Claims.Blazor8.Services;

public record struct GetMeResponse(Guid CustomerId, string Role, string License, string Name);
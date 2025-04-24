namespace Berrilan.Claims.Blazor8;

public record struct GetMeResponse(
    Guid CustomerId,
    string Role,
    string License,
    string Name
);

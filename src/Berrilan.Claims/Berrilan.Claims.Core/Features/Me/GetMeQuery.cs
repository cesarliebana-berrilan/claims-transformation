using Berrilan.Claims.Core.Domain;
using MediatR;

namespace Berrilan.Claims.Core.Features.Me;

public record struct GetMeQuery() : IRequest<GetMeResponse>;

public record struct GetMeResponse(
    Guid CustomerId,
    Role Role,
    License License,
    string Name
);
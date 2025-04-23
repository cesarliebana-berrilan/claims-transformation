using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Berrilan.Claims.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Berrilan.Claims.Core.Features.Me;

public class GetMeHandler(AppDbContext _dbContext) : IRequestHandler<GetMeQuery, GetMeResponse>
{
    public async Task<GetMeResponse> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == Guid.Empty, cancellationToken)
            ?? throw new ItemNotFoundException(nameof(User), string.Empty);

        return new GetMeResponse(user.CustomerId, user.Role, user.License, string.Empty);
    }
}

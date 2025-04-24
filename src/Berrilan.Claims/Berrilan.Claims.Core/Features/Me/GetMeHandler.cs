using Berrilan.Claims.Core.Domain;
using Berrilan.Claims.Core.Exceptions;
using Berrilan.Claims.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Berrilan.Claims.Core.Features.Me;

public class GetMeHandler(AppDbContext dbContext, IUserContext userContext) : IRequestHandler<GetMeQuery, GetMeResponse>
{
    public async Task<GetMeResponse> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userContext.UserId, cancellationToken)
            ?? throw new ItemNotFoundException(nameof(User), userContext.UserId.ToString());

        return new GetMeResponse(user.CustomerId, user.Role, user.License, string.Empty);
    }
}

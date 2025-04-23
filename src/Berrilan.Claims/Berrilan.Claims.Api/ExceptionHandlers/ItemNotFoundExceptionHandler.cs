using Berrilan.Claims.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Berrilan.Claims.Api.ExceptionHandlers;

internal sealed class ItemNotFoundExceptionHandler(ILogger<ItemNotFoundExceptionHandler> _logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ItemNotFoundException) return false;

        _logger.LogWarning("ItemNotFoundException {Error}", exception.Message);

        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
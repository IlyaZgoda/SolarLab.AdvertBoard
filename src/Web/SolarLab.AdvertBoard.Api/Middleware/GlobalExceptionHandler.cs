using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Infrastructure.Exceptions;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Api.Middleware
{
    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService,
       ILogger<GlobalExceptionHandler> logger,
       ErrorToHttpMapper mapper,
       ProblemDetailsFactory problemDetailsFactory)
       : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

            Error error = exception switch
            {
                _ => new Error(ErrorTypes.ValidationError, "Validation error"),
            };

            var statusCode = mapper.Map(error);
            var problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext, statusCode, title: error.Code, detail: exception.Message);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

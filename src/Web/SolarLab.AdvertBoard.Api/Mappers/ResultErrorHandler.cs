using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Api.Mappers
{
    public class ResultErrorHandler(
        IHttpContextAccessor httpContextAccessor, 
        ErrorToHttpMapper errorToHttpMapper, 
        ProblemDetailsFactory problemDetailsFactory)
    {
        public IActionResult Handle(Error error)
        {
            var statusCode = errorToHttpMapper.Map(error);
            var problemDetails = problemDetailsFactory.CreateProblemDetails(
                httpContext: httpContextAccessor.HttpContext!, 
                statusCode: statusCode, 
                detail: error.Description,
                title: error.Code);

            return new ObjectResult(problemDetails);
        }
    }
}

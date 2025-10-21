using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Api.Mappers
{
    /// <summary>
    /// Обработчик ошибок <see cref="Error"/>/>.
    /// </summary>
    /// <param name="httpContextAccessor">Акссессор http контекста.</param>
    /// <param name="errorToHttpMapper">Маппер ошибок.</param>
    /// <param name="problemDetailsFactory">Фабрика для создания Problem details.</param>
    public class ResultErrorHandler(
        IHttpContextAccessor httpContextAccessor, 
        ErrorToHttpMapper errorToHttpMapper, 
        ProblemDetailsFactory problemDetailsFactory)
    {
        /// <summary>
        /// Обрабатывает ошибку.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns></returns>
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

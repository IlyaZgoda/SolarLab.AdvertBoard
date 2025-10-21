using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Api.Mappers
{
    /// <summary>
    /// Маппер для преобразования доменных ошибок в соответствующие HTTP статус-коды.
    /// </summary>

    public class ErrorToHttpMapper
    {
        /// <summary>
        /// Преобразует доменную ошибку в соответствующий HTTP статус-код.
        /// </summary>
        /// <param name="error">Доменная ошибка.</param>
        /// <returns>HTTP статус-код, соответствующий типу ошибки.</returns>
        public int Map(Error error) =>
         error.Code switch
         {
             ErrorTypes.InvalidCredentials => StatusCodes.Status401Unauthorized,
             ErrorTypes.NotFound => StatusCodes.Status404NotFound,
             ErrorTypes.Forbidden => StatusCodes.Status403Forbidden,
             ErrorTypes.Timeout => StatusCodes.Status408RequestTimeout,
             ErrorTypes.NetworkError => StatusCodes.Status503ServiceUnavailable,
             ErrorTypes.ValidationError => StatusCodes.Status400BadRequest,
             ErrorTypes.UnprocessableEntity => StatusCodes.Status422UnprocessableEntity,
             ErrorTypes.InternalServerError => StatusCodes.Status500InternalServerError,
             ErrorTypes.AlreadyExists => StatusCodes.Status409Conflict,
             _ => StatusCodes.Status400BadRequest
         };
    }
}

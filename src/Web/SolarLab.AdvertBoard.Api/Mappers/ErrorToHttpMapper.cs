using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Api.Mappers
{
    public class ErrorToHttpMapper
    {
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
             _ => StatusCodes.Status500InternalServerError
         };
    }
}

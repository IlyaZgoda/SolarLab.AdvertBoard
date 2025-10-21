namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Статический класс с константами типов ошибок.
    /// </summary>
    public static class ErrorTypes
    {
        public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
        public const string NotFound = "NOT_FOUND";
        public const string Forbidden = "AUTH_FORBIDDEN";
        public const string Timeout = "TIMEOUT";
        public const string NetworkError = "NETWORK_ERROR";
        public const string ValidationError = "VALIDATION_ERROR";
        public const string UnprocessableEntity = "UNPROCESSABLE_ENTITY";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string AlreadyExists = "ALREADY_EXISTS";
    }
}

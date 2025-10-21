namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    /// <summary>
    /// Провайдер для получения идентификатора текущего аутентифицированного пользователя.
    /// </summary>
    public interface IUserIdentifierProvider
    {
        /// <summary>
        /// Получает идентификатор текущего аутентифицированного пользователя из системы аутентификации.
        /// </summary>
        string IdentityUserId { get; }
    }
}

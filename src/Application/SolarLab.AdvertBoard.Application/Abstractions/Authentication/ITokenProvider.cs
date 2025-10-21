namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    /// <summary>
    /// Провайдер для создания JWT токенов аутентификации.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Создает JWT токен для пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя в системе аутентификации.</param>
        /// <param name="email">Email пользователя, который используется в качестве логина.</param>
        /// <returns>JWT токен в виде строки.</returns>
        string Create(string id, string email);
    }
}

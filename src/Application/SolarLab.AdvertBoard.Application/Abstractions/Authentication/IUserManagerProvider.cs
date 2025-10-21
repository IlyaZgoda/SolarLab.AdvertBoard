using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    /// <summary>
    /// Провайдер для управления пользователями в системе аутентификации (Identity).
    /// </summary>
    public interface IUserManagerProvider
    {
        /// <summary>
        /// Создает нового пользователя в системе аутентификации.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>
        /// Успешный результат с идентификатором созданного пользователя или ошибку создания.
        /// </returns>
        Task<Result<string>> CreateIdentityUserAsync(string email, string password);

        /// <summary>
        /// Проверяет учетные данные пользователя в системе аутентификации.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>
        /// Успешный результат с идентификатором пользователя при валидных учетных данных или ошибку аутентификации.
        /// </returns>
        Task<Result<string>> ValidateIdentityUserAsync(string email, string password);

        /// <summary>
        /// Генерирует токен подтверждения email для пользователя.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Токен подтверждения email.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(string email);

        /// <summary>
        /// Получает email пользователя по его идентификатору в системе аутентификации.
        /// </summary>
        /// <param name="identityUserId">Идентификатор пользователя в системе аутентификации.</param>
        /// <returns>Email пользователя.</returns>
        Task<string> GetEmailByIdAsync(string identityUserId);

        /// <summary>
        /// Подтверждает email пользователя с использованием токена.
        /// </summary>
        /// <param name="identityUserId">Идентификатор пользователя в системе аутентификации.</param>
        /// <param name="token">Токен подтверждения email.</param>
        /// <returns>
        /// Успешный результат если email подтвержден, или ошибку подтверждения.
        /// </returns>
        Task<Result> ConfirmEmail(string identityUserId, string token);

        /// <summary>
        /// Проверяет, подтвержден ли email пользователя.
        /// </summary>
        /// <param name="identityUserId">Идентификатор пользователя в системе аутентификации.</param>
        /// <returns>true если email подтвержден, иначе false.</returns>
        Task<bool> IsEmailConfirmed(string identityUserId);
    }
}

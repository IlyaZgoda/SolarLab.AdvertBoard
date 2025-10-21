using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.ConfirmEmail
{
    /// <summary>
    /// Команда для подтверждения почты пользователя.
    /// </summary>
    /// <param name="IdentityUserId">Идентификатор пользователя в системе аутентификации.</param>
    /// <param name="Token">Токен подтверждения почты.</param>
    public record ConfirmEmailCommand(string IdentityUserId, string Token) : ICommand<JwtResponse>;
}

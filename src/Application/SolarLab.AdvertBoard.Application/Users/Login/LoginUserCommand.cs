using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    /// <summary>
    /// Команда для аутентификации пользователя.
    /// </summary>
    /// <param name="Email">Email.</param>
    /// <param name="Password">Пароль.</param>
    public record LoginUserCommand(string Email, string Password) : ICommand<JwtResponse>;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Users;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
    /// <summary>
    /// Команда для регистрации пользователя.
    /// </summary>
    /// <param name="Email">Email.</param>
    /// <param name="Password">Пароль.</param>
    /// <param name="FirstName">Имя.</param>
    /// <param name="LastName">Фамилия.</param>
    /// <param name="MiddleName">Отчество (опционально).</param>
    /// <param name="ContactEmail">Контактный Email (опционально).</param>
    /// <param name="PhoneNumber">Контактный номер телефона (опционально).</param>
    public record RegisterUserCommand(
        string Email, 
        string Password, 
        string FirstName, 
        string LastName, 
        string? MiddleName, 
        string? ContactEmail,
        string? PhoneNumber) 
        : ICommand<UserIdResponse>;
}

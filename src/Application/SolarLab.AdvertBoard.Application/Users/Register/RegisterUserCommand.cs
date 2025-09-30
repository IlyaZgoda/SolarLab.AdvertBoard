using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Users;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
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

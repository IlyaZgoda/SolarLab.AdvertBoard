using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    public record LoginUserCommand(string Email, string Password) : ICommand<JwtResponse>;
}

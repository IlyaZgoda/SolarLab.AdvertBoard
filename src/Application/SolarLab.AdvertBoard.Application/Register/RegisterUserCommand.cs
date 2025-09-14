using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Register
{
    public record RegisterUserCommand(string Email, string Password) : ICommand<TokenResponse>;
}

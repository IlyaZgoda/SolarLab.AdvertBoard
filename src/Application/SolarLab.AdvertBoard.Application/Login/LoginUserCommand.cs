using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Login
{
    public record LoginUserCommand(string Email, string Password) : ICommand<TokenResponse>;
}

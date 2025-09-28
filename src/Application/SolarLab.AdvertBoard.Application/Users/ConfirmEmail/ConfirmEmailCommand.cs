using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.ConfirmEmail
{
    public record ConfirmEmailCommand(string IdentityUserId, string Token) : ICommand<JwtResponse>;
}

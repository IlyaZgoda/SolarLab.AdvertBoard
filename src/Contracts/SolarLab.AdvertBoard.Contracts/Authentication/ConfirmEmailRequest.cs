using System.Windows.Input;

namespace SolarLab.AdvertBoard.Contracts.Authentication
{
    public record ConfirmEmailRequest(string UserId, string EncodedToken);
}

using SolarLab.AdvertBoard.Contracts.Links;

namespace SolarLab.AdvertBoard.Application.Abstractions.Links
{
    public interface IUriGenerator
    {
        string GenerateEmailConfirmationUri(ConfirmationUriRequest confirmationUriRequest);
    }
}

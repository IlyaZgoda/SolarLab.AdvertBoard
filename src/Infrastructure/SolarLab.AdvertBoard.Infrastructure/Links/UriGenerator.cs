using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SolarLab.AdvertBoard.Application.Abstractions.Links;
using SolarLab.AdvertBoard.Contracts.Links;
using SolarLab.AdvertBoard.Domain.Users;
using System.Text;

namespace SolarLab.AdvertBoard.Infrastructure.Links
{
    public class UriGenerator(IOptions<UriGeneratorOptions> options) : IUriGenerator
    {
        private readonly UriGeneratorOptions _options = options.Value;
        public string GenerateEmailConfirmationUri(ConfirmationUriRequest confirmationUriRequest)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationUriRequest.Token));
            return $"{_options.BaseUri}/api/users/confirm-email?userId={confirmationUriRequest.UserId}&encodedToken={encodedToken}";
        }
    }
}

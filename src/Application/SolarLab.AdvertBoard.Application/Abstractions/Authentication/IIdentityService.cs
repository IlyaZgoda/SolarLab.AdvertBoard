using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<Result<string>> CreateIdentityUserAsync(string email, string password);

        Task<Result<string>> ValidateIdentityUserAsync(string email, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(string email);

        Task<string> GetEmailByIdAsync(string identityUserId);

        Task<Result> ConfirmEmail(string identityUserId, string token);

        Task<bool> IsEmailConfirmed(string identityUserId);
    }
}

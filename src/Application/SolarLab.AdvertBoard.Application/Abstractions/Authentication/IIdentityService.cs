namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<string> CreateIdentityUserAsync(string email, string password);

        Task<string?> ValidateIdentityUserAsync(string email, string password);

        Task<string?> GenerateEmailConfirmationTokenAsync(string email);

        Task<string?> GetEmailByIdAsync(string identityUserId);

        Task<bool> ConfirmEmail(string identityUserId, string token);

        Task<bool> IsEmailConfirmed(string identityUserId);
    }
}

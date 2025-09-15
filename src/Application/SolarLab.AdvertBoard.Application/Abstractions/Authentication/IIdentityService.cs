namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<string> CreateIdentityUserAsync(string email, string password);

        Task<string?> ValidateIdentityUserAsync(string email, string password);


    }
}

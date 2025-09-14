namespace SolarLab.AdvertBoard.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(string email, string password);

        Task<string?> ValidateUserAsync(string email, string password);


    }
}

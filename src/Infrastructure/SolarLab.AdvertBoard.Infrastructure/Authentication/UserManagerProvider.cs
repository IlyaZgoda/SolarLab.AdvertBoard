using Microsoft.AspNetCore.Identity;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Infrastructure.Exceptions;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Infrastructure.Authentication
{
    /// <summary>
    /// Провайдер для управления пользователями из системы аутентификации.
    /// </summary>
    /// <param name="userManager">Менеджер для управления пользователями из сестемы аутентификации (Identity).</param>
    public class UserManagerProvider(UserManager<IdentityUser> userManager) : IUserManagerProvider
    {
        private static Error IncorrectLoginOrPassword = new(ErrorTypes.ValidationError, "Incorrect login or password");

        /// <inheritdoc/>
        public async Task<Result<string>> CreateIdentityUserAsync(string email, string password)
        {
            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email,
            };

            var result = await userManager.CreateAsync(identityUser, password);

            if(!result.Succeeded)
            {
                return Result.Failure<string>(new Error(result.Errors.First().Code, result.Errors.First().Description));
            }

            return identityUser.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteIdentityUserAsync(string identityUserId)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId);

            if(identityUser is null)
            {
                return false;
            } 

            var result = await userManager.DeleteAsync(identityUser);

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors.First().Description);
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<Result<string>> ValidateIdentityUserAsync(string email, string password)
        {
            var identityUser = await userManager.FindByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<string>(IncorrectLoginOrPassword);
            }

            var isValid = await userManager.CheckPasswordAsync(identityUser, password);
            
            return isValid 
                ? identityUser.Id
                : Result.Failure<string>(IncorrectLoginOrPassword);
        }

        /// <inheritdoc/>
        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var identityUser = await userManager.FindByEmailAsync(email)
                ?? throw new ArgumentException("Identity user not found");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            return token;
        }

        public async Task<string> GetEmailByIdAsync(string identityUserId)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId) 
                ?? throw new ArgumentException("Identity user not found");

            return identityUser.Email!;
        }

        /// <inheritdoc/>
        public async Task<Result> ConfirmEmail(string identityUserId, string token)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId)
                ?? throw new ArgumentException("Identity user not found");

            var result = await userManager.ConfirmEmailAsync(identityUser, token);

            if (!result.Succeeded)
            {
                return Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description));
            }

            return Result.Success();
        }

        /// <inheritdoc/>
        public async Task<bool> IsEmailConfirmed(string identityUserId)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId)
                ?? throw new ArgumentException("Identity user not found");

            return identityUser.EmailConfirmed;
        }
    }
}

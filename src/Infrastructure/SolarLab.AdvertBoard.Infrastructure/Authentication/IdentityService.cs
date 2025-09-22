using Microsoft.AspNetCore.Identity;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.ConfirmEmail;
using SolarLab.AdvertBoard.Infrastructure.Exceptions;

namespace SolarLab.AdvertBoard.Infrastructure.Authentication
{
    public class IdentityService(UserManager<IdentityUser> userManager) : IIdentityService
    {
        public async Task<string> CreateIdentityUserAsync(string email, string password)
        {
            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email,
            };

            var result = await userManager.CreateAsync(identityUser, password);

            if(!result.Succeeded)
            {
                throw new IdentityException(result.Errors.First().Description);
            }

            return identityUser.Id;
        }

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

        public async Task<string?> ValidateIdentityUserAsync(string email, string password)
        {
            var identityUser = await userManager.FindByEmailAsync(email); 
            
            if (identityUser is null)
            {
                return null;
            }

            var isValid = await userManager.CheckPasswordAsync(identityUser, password);
            
            return isValid ? identityUser.Id : null;
        }

        public async Task<string?> GenerateEmailConfirmationTokenAsync(string email)
        {
            var identityUser = await userManager.FindByEmailAsync(email);

            if (identityUser is null)
            {
                return null;
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            return token;
        }

        public async Task<string?> GetEmailByIdAsync(string identityUserId)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId);

            return identityUser?.Email;
        }

        public async Task<bool> ConfirmEmail(string identityUserId, string token)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId);

            if (identityUser is null)
            {
                return false;
            }

            var result = await userManager.ConfirmEmailAsync(identityUser, token);

            return result.Succeeded;
        }

        public async Task<bool> IsEmailConfirmed(string identityUserId)
        {
            var identityUser = await userManager.FindByIdAsync(identityUserId);

            if (identityUser is null)
            {
                return false;
            }

            return identityUser.EmailConfirmed;
        }
    }
}

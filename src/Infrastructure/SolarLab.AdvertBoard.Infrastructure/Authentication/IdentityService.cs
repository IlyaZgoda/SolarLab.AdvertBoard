using Microsoft.AspNetCore.Identity;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
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
    }
}

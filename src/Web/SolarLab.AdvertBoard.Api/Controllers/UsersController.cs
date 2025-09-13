using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Infrastructure.Authentication;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class UsersController(TokenProvider tokenProvider, UserManager<IdentityUser> userManager) : ControllerBase
    {
        [HttpGet("api/users/login")]
        [Authorize]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpGet("api/users/register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}

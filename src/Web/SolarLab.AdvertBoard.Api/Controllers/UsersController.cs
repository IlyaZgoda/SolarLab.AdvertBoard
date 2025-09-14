using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Login;
using SolarLab.AdvertBoard.Application.Register;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Infrastructure.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class UsersController(ITokenProvider tokenProvider, UserManager<IdentityUser> userManager, IMediator mediator) : ControllerBase
    {
        [HttpGet("api/users/login")]

        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            Result<TokenResponse> result = await Result.Create(loginRequest, Error.None)
                .Map(request => new LoginUserCommand(loginRequest.Email, loginRequest.Password))
                .Bind(command => mediator.Send(command));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpGet("api/users/register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerRequest)
        {
            Result<TokenResponse> result = await Result.Create(registerRequest, Error.None)
                .Map(request => new RegisterUserCommand(registerRequest.Email, registerRequest.Password))
                .Bind(command => mediator.Send(command));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}

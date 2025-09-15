using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Application.Login;
using SolarLab.AdvertBoard.Application.Register;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.Domain.Users.Events;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class UsersController(IMediator mediator) : ControllerBase
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
            Result<UserResponse> result = await Result.Create(registerRequest, Error.None)
                .Map(request => new RegisterUserCommand(registerRequest.Email, registerRequest.Password, registerRequest.FirstName, registerRequest.FirstName, registerRequest.MiddleName, registerRequest.PhoneNumber))
                .Bind(command => mediator.Send(command));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        } 
    }
}

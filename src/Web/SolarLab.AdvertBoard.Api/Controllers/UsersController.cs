using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Abstractions.Links;
using SolarLab.AdvertBoard.Application.Abstractions.Notifications;
using SolarLab.AdvertBoard.Application.ConfirmEmail;
using SolarLab.AdvertBoard.Application.Login;
using SolarLab.AdvertBoard.Application.Register;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Contracts.Links;
using SolarLab.AdvertBoard.Contracts.Mails;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class UsersController(
        IMediator mediator, 
        IEmailNotificationSender emailNotificationSender, 
        IUriGenerator uriGenerator, 
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [HttpPost(ApiRoutes.Users.Login)]
        [ProducesResponseType(typeof(JwtResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Login(LoginRequest loginRequest) =>   
            await Result.Create(loginRequest, Error.None)
                .Map(request => new LoginUserCommand(loginRequest.Email, loginRequest.Password))
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));

        [HttpPost(ApiRoutes.Users.Register)]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Register(RegisterUserRequest registerRequest) =>
            await Result.Create(registerRequest, Error.None)
                .Map(request => new RegisterUserCommand(
                    registerRequest.Email, 
                    registerRequest.Password, 
                    registerRequest.FirstName, 
                    registerRequest.LastName, 
                    registerRequest.MiddleName,
                    registerRequest.ContactEmail,
                    registerRequest.PhoneNumber))
                .Bind(command => mediator.Send(command))
                .Match(user => Ok(user.UserId), error => resultErrorHandler.Handle(error));
 
        [HttpGet(ApiRoutes.Users.ConfirmEmail)]
        [ProducesResponseType(typeof(JwtResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest confirmEmailRequest)
        {
            var encodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.EncodedToken));

            return await Result.Create(confirmEmailRequest, Error.None)
                .Map(request => new ConfirmEmailCommand(confirmEmailRequest.UserId, encodedToken))
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));
        }

        [HttpGet("api/users/send-email")]
        public async Task<IActionResult> SendEmail()
        {
            var uri = uriGenerator.GenerateEmailConfirmationUri(new ConfirmationUriRequest("zxcv-asdf-qwer-sdfg-1234-asdf-zxcv-asdf","asdkuh34kjubanskld;oq2enlkajsnd"));
            await emailNotificationSender.SendConfirmationEmail(new ConfirmationEmail("zgoda-games@mail.ru", uri));

            return Ok();
        }
    }
}

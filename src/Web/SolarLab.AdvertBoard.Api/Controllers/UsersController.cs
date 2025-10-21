using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Users.ConfirmEmail;
using SolarLab.AdvertBoard.Application.Users.Login;
using SolarLab.AdvertBoard.Application.Users.Register;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления операциями с пользователями.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    /// <param name="resultErrorHandler">Обработчик ошибок для преобразования в HTTP ответы.</param>
    [ApiController]
    public class UsersController(
        IMediator mediator, 
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        /// <summary>
        /// Выполняет аутентификацию пользователя.
        /// </summary>
        /// <param name="loginRequest">Данные для входа (email и пароль).</param>
        /// <returns>
        /// JWT токен при успешной аутентификации.
        /// Ошибку 400 при неверных учетных данных или ошибке валидации.
        /// Ошибку 500 при внутренней ошибке сервера.
        /// </returns>
        [HttpPost(ApiRoutes.Users.Login)]
        [ProducesResponseType(typeof(JwtResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Login([FromBody]LoginUserRequest loginRequest) =>   
            await Result.Create(loginRequest, Error.None)
                .Map(request => new LoginUserCommand(loginRequest.Email, loginRequest.Password))
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Регистрирует нового пользователя в системе.
        /// </summary>
        /// <param name="registerRequest">Данные для регистрации пользователя.</param>
        /// <returns>
        /// Идентификатор созданного пользователя при успешной регистрации.
        /// Ошибку 400 при неверных данных или нарушении бизнес-правил.
        /// Ошибку 500 при внутренней ошибке сервера.
        /// </returns>
        [HttpPost(ApiRoutes.Users.Register)]
        [ProducesResponseType(typeof(UserIdResponse), 200)]
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

        /// <summary>
        /// Подтверждает email пользователя с использованием токена.
        /// </summary>
        /// <param name="confirmEmailRequest">Запрос с идентификатором пользователя и токеном подтверждения.</param>
        /// <returns>
        /// JWT токен при успешном подтверждении email.
        /// Ошибку 400 при неверном или просроченном токене.
        /// Ошибку 500 при внутренней ошибке сервера.
        /// </returns>
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
    }
}

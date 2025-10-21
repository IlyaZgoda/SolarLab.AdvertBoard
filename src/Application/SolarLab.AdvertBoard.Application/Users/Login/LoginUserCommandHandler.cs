using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    /// <summary>
    /// Обработчик комманды <see cref="LoginUserCommand"/>
    /// </summary>
    /// <param name="userManagerProvider">Провайдер для управления пользователя в системе аутентификации.</param>
    /// <param name="tokenProvider">Провайдер для создания JWT.</param>
    public class LoginUserCommandHandler(IUserManagerProvider userManagerProvider, ITokenProvider tokenProvider)
        : ICommandHandler<LoginUserCommand, JwtResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<JwtResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var identityUserId = await userManagerProvider.ValidateIdentityUserAsync(request.Email, request.Password);

            if (identityUserId.IsFailure)
            {
                return Result.Failure<JwtResponse>(identityUserId.Error);
            }

            var isConfirmed = await userManagerProvider.IsEmailConfirmed(identityUserId.Value);

            if (!isConfirmed)
            {
                return Result.Failure<JwtResponse>(new Error(ErrorTypes.ValidationError, "Email is not confirmed"));
            }

            var token = tokenProvider.Create(identityUserId.Value, request.Email);

            return new JwtResponse(token);
        }
    }
}

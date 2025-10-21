using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Users.ConfirmEmail
{
    /// <summary>
    /// Обработчик команды <see cref="ConfirmEmailCommand"/>
    /// </summary>
    /// <param name="identityService">Провайдер для получения идентификатора текущего аутентифицированного пользователя.</param>
    /// <param name="tokenProvider">Провайдер для создания JWT.</param>
    public class ConfirmEmailCommandHandler(IUserManagerProvider identityService, ITokenProvider tokenProvider) 
        : ICommandHandler<ConfirmEmailCommand, JwtResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<JwtResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.ConfirmEmail(request.IdentityUserId, request.Token);

            if (result.IsFailure)
            {
                return Result.Failure<JwtResponse>(result.Error);
            }

            var email = await identityService.GetEmailByIdAsync(request.IdentityUserId);

            var jwt = tokenProvider.Create(request.IdentityUserId, email);

            return new JwtResponse(jwt);
        }
    }
}

using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Login
{
    public class LoginUserCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, TokenResponse>
    {
        public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var identityUserId = await identityService.ValidateUserAsync(request.Email, request.Password);

            if (identityUserId is null)
            {
                return Result.Failure<TokenResponse>(new Error(ErrorTypes.ValidationError, "Incorrect email or password"));
            }

            var token = tokenProvider.Create(identityUserId, request.Email);

            return new TokenResponse(token);
        }
    }
}

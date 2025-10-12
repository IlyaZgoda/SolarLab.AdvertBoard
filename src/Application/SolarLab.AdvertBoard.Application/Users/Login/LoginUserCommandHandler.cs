using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    public class LoginUserCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider)
        : ICommandHandler<LoginUserCommand, JwtResponse>
    {
        public async Task<Result<JwtResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var identityUserId = await identityService.ValidateIdentityUserAsync(request.Email, request.Password);

            if (identityUserId.IsFailure)
            {
                return Result.Failure<JwtResponse>(identityUserId.Error);
            }

            var isConfirmed = await identityService.IsEmailConfirmed(identityUserId.Value);

            if (!isConfirmed)
            {
                return Result.Failure<JwtResponse>(new Error(ErrorTypes.ValidationError, "Email is not confirmed"));
            }

            var token = tokenProvider.Create(identityUserId.Value, request.Email);

            return new JwtResponse(token);
        }
    }
}

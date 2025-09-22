using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Login
{
    public class LoginUserCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, JwtResponse>
    {
        public async Task<Result<JwtResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var identityUserId = await identityService.ValidateIdentityUserAsync(request.Email, request.Password);

            if (identityUserId is null)
            {
                return Result.Failure<JwtResponse>(new Error(ErrorTypes.ValidationError, "Incorrect email or password"));
            }

            var isConfirmed = await identityService.IsEmailConfirmed(identityUserId);

            if (!isConfirmed)
            {
                throw new Exception();
            }

            var token = tokenProvider.Create(identityUserId, request.Email);

            return new JwtResponse(token);
        }
    }
}

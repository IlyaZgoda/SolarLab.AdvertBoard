using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Register
{
    public class RegisterUserCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider) : ICommandHandler<RegisterUserCommand, TokenResponse>
    {
        public async Task<Result<TokenResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var identityUserId = await identityService.CreateUserAsync(request.Email, request.Password);

            var token = tokenProvider.Create(identityUserId, request.Email);

            return new TokenResponse(token);
        }
    }
}

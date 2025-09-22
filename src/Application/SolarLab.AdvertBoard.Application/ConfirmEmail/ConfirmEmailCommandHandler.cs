using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(IIdentityService identityService, ITokenProvider tokenProvider) : ICommandHandler<ConfirmEmailCommand, JwtResponse>
    {
        public async Task<Result<JwtResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.ConfirmEmail(request.IdentityUserId, request.Token);

            if (!result)
            {
                throw new ArgumentException();
            }

            var email = await identityService.GetEmailByIdAsync(request.IdentityUserId) 
                ?? throw new ArgumentException();

            var jwt = tokenProvider.Create(request.IdentityUserId, email);

            return new JwtResponse(jwt);
        }
    }
}

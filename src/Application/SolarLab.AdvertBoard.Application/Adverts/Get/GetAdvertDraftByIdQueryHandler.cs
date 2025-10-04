using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetAdvertByIdQueryHandler(
        IAdvertReadService advertReadService, 
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository) : IQueryHandler<GetAdvertDraftByIdQuery, AdvertDraftResponse>
    {
        public async Task<Result<AdvertDraftResponse>> Handle(GetAdvertDraftByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await advertReadService.GetAdvertDraftDetailsByIdAsync(new AdvertId(request.Id));

            if (response.HasNoValue)
            {
                return Result.Failure<AdvertDraftResponse>(AdvertErrors.NotFound);
            }

            if (userIdentifierProvider.IdentityUserId != await userRepository.GetIdentityIdByUserIdAsync(new UserId(response.Value.AuthorId)))
            {
                return Result.Failure<AdvertDraftResponse>(AdvertErrors.NotFound);
            }

            return response.Value;
        }
    }
}

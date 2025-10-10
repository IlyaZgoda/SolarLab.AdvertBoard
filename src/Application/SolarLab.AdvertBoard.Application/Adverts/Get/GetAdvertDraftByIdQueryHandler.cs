using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetAdvertByIdQueryHandler(
       IAdvertReadProvider advertReadService,
       IUserIdentifierProvider userIdentifierProvider,
       IUserRepository userRepository) : IQueryHandler<GetAdvertDraftByIdQuery, AdvertDraftDetailsResponse>
    {
        public async Task<Result<AdvertDraftDetailsResponse>> Handle(GetAdvertDraftByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await advertReadService.GetAdvertDraftDetailsByIdAsync(new AdvertId(request.Id));

            if (response.HasNoValue)
            {
                return Result.Failure<AdvertDraftDetailsResponse>(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(new UserId(response.Value.AuthorId), userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure<AdvertDraftDetailsResponse>(AdvertErrors.NotFound);
            }

            return response.Value;
        }
    }
}

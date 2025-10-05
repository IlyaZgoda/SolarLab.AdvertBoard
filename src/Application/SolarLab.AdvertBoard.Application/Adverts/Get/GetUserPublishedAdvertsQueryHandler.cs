using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetUserPublishedAdvertsQueryHandler(
        IAdvertReadService advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserPublishedAdvertsQuery, PublishedAdvertsResponse>
    {
        public async Task<Result<PublishedAdvertsResponse>> Handle(GetUserPublishedAdvertsQuery request, CancellationToken cancellationToken)
        {
            var identityUserId = userIdentifierProvider.IdentityUserId;

            var response = await advertReadService.GetUserPublishedAdverts(identityUserId, request.Page ?? 1, request.PageSize ?? 20);

            return response.Value;
        }
    }
}

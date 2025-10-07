using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetUserPublishedAdvertsQueryHandler(
        IAdvertReadService advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserPublishedAdvertsQuery, PaginationCollection<PublishedAdvertItem>>
    {
        public async Task<Result<PaginationCollection<PublishedAdvertItem>>> Handle(GetUserPublishedAdvertsQuery request, CancellationToken cancellationToken) =>
            await advertReadService.GetUserPublishedAdverts(userIdentifierProvider.IdentityUserId, request.Page, request.PageSize);
    }
}

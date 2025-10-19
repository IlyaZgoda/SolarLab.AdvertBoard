using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Application.Adverts.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserPublishedAdverts
{
    public class GetUserPublishedAdvertsQueryHandler(
        IAdvertReadProvider advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserPublishedAdvertsQuery, PaginationCollection<PublishedAdvertItem>>
    {
        public async Task<Result<PaginationCollection<PublishedAdvertItem>>> Handle(GetUserPublishedAdvertsQuery request, CancellationToken cancellationToken)
        {
            var byUserIdentity = new AdvertByUserIdentitySpec(userIdentifierProvider.IdentityUserId);
            var published = new PublishedAdvertSpec();
            var spec = byUserIdentity.And(published);

            return await advertReadService.GetUserPublishedAdverts(request.Page, request.PageSize, spec);
        }
            
    }
}

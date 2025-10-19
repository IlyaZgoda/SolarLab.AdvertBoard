using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Application.Adverts.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserDrafts
{
    public class GetUserAdvertDraftsQueryHandler(
        IAdvertReadProvider advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserAdvertDraftsQuery, PaginationCollection<AdvertDraftItem>>
    {
        public async Task<Result<PaginationCollection<AdvertDraftItem>>> Handle(GetUserAdvertDraftsQuery request, CancellationToken cancellationToken)
        {
            var byUserIdentity = new AdvertByUserIdentitySpec(userIdentifierProvider.IdentityUserId);
            var drafts = new AdvertDraftSpec();
            var spec = byUserIdentity.And(drafts);

            return await advertReadService.GetUserAdvertDrafts(request.Page, request.PageSize, spec);
        }
            
    }
}

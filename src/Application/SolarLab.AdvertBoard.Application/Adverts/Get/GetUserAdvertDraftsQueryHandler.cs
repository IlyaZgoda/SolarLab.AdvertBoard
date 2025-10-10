using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetUserAdvertDraftsQueryHandler(
        IAdvertReadProvider advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserAdvertDraftsQuery, PaginationCollection<AdvertDraftItem>>
    {
        public async Task<Result<PaginationCollection<AdvertDraftItem>>> Handle(GetUserAdvertDraftsQuery request, CancellationToken cancellationToken) =>
            await advertReadService.GetUserAdvertDrafts(userIdentifierProvider.IdentityUserId, request.Page, request.PageSize); 
    }
}

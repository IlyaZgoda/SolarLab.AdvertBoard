using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetUserAdvertDraftsQuery(int? Page, int? PageSize) : IQuery<AdvertDraftsResponse>;

    public class GetUserAdvertDraftsQueryHandler(
        IAdvertReadService advertReadService, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserAdvertDraftsQuery, AdvertDraftsResponse>
    {
        public async Task<Result<AdvertDraftsResponse>> Handle(GetUserAdvertDraftsQuery request, CancellationToken cancellationToken)
        {
            var userIdentityId = userIdentifierProvider.IdentityUserId;

            var response = await advertReadService.GetUserAdvertDrafts(userIdentityId, request.Page ?? 1, request.PageSize ?? 20);

            return response.Value;
        }
    }
}

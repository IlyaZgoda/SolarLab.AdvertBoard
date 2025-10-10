using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetPublishedAdvertsByFilterQueryHandler(IAdvertReadProvider advertReadService) 
        : IQueryHandler<GetPublishedAdvertsByFilterQuery, PaginationCollection<PublishedAdvertItem>>
    {
        public async Task<Result<PaginationCollection<PublishedAdvertItem>>> Handle(GetPublishedAdvertsByFilterQuery request, CancellationToken cancellationToken) =>
            await advertReadService.GetPublishedAdvertsByFilterAsync(request.Filter);
    }
}

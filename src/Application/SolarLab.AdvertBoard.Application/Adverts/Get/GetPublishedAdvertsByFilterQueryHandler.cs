using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetPublishedAdvertsByFilterQueryHandler(IAdvertReadService advertReadService) 
        : IQueryHandler<GetPublishedAdvertsByFilterQuery, PublishedAdvertsResponse>
    {
        public async Task<Result<PublishedAdvertsResponse>> Handle(GetPublishedAdvertsByFilterQuery request, CancellationToken cancellationToken)
        {
            var response = await advertReadService.GetPublishedAdvertsByFilterAsync(request.Count ?? 1, request.PageSize ?? 20);

            return response.Value;
        }
    }
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadServices;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public class GetPublishedAdvertDetailsByIdQueryHandler(IAdvertReadService advertReadService) 
        : IQueryHandler<GetPublishedAdvertDetailsByIdQuery, PublishedAdvertDetailsResponse>
    {
        public async Task<Result<PublishedAdvertDetailsResponse>> Handle(GetPublishedAdvertDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await advertReadService.GetPublishedAdvertDetailsByIdAsync(new AdvertId(request.Id));

            if (response.HasNoValue)
            {
                return Result.Failure<PublishedAdvertDetailsResponse>(AdvertErrors.NotFound);
            }

            return response.Value;
        }
    }
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Application.Adverts.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertDetailsById
{
    /// <summary>
    /// Обработчик запроса <see cref="GetPublishedAdvertDetailsByIdQuery"/>.
    /// </summary>
    /// <param name="advertReadProvider">Провайдер для чтения данных объявлений</param>
    public class GetPublishedAdvertDetailsByIdQueryHandler(IAdvertReadProvider advertReadProvider) 
        : IQueryHandler<GetPublishedAdvertDetailsByIdQuery, PublishedAdvertDetailsResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<PublishedAdvertDetailsResponse>> Handle(GetPublishedAdvertDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var byId = new AdvertWithIdSpec(request.Id);
            var published = new PublishedAdvertSpec();
            var spec = byId.And(published);

            var response = await advertReadProvider.GetPublishedAdvertDetailsByIdAsync(spec);

            if (response.HasNoValue)
            {
                return Result.Failure<PublishedAdvertDetailsResponse>(AdvertErrors.NotFound);
            }

            return response.Value;
        }
    }
}

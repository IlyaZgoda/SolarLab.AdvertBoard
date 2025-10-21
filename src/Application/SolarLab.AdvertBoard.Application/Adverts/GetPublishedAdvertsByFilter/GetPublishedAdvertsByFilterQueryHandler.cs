using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertsByFilter
{
    /// <summary>
    /// Обработчик запроса <see cref="GetPublishedAdvertsByFilterQuery"/>.
    /// </summary>
    /// <param name="advertReadProvider">Провайдер для чтения данных объявлений.</param>
    public class GetPublishedAdvertsByFilterQueryHandler(IAdvertReadProvider advertReadProvider) 
        : IQueryHandler<GetPublishedAdvertsByFilterQuery, PaginationCollection<PublishedAdvertItem>>
    {
        /// <inheritdoc/>
        public async Task<Result<PaginationCollection<PublishedAdvertItem>>> Handle(GetPublishedAdvertsByFilterQuery request, CancellationToken cancellationToken) =>
            await advertReadProvider.GetPublishedAdvertsByFilterAsync(request.Filter);
    }
}

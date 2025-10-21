using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertsByFilter
{
    /// <summary>
    /// Запрос для получения опубликованных объявлений по фильтру.
    /// </summary>
    /// <param name="Filter">Параметры фильтрации.</param>
    public record GetPublishedAdvertsByFilterQuery(AdvertFilterRequest Filter) : IQuery<PaginationCollection<PublishedAdvertItem>>;
}

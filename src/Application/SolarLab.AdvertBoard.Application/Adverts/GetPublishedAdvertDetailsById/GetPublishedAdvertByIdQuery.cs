using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertDetailsById
{
    /// <summary>
    /// Запрос для получения деталей опубликованного объявления по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор объявления.</param>
    public record GetPublishedAdvertDetailsByIdQuery(Guid Id) : IQuery<PublishedAdvertDetailsResponse>;
}

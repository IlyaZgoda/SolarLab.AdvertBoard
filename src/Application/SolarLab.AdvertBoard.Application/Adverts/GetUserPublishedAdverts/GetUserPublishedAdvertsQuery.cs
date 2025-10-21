using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserPublishedAdverts
{
    /// <summary>
    /// Запрос для получения опубликованных объявлений пользователя.
    /// </summary>
    /// <param name="Page">Номер страницы.</param>
    /// <param name="PageSize">Размер страницы.</param>
    public record GetUserPublishedAdvertsQuery(int Page = 1, int PageSize = 20) : IQuery<PaginationCollection<PublishedAdvertItem>>;
}

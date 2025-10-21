using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserDrafts
{
    /// <summary>
    /// Запрос для получения черновиков объявлений пользователя.
    /// </summary>
    /// <param name="Page">Номер страницы.</param>
    /// <param name="PageSize">Размер страницы.</param>
    public record GetUserAdvertDraftsQuery(int Page = 1, int PageSize = 20) : IQuery<PaginationCollection<AdvertDraftItem>>;
}

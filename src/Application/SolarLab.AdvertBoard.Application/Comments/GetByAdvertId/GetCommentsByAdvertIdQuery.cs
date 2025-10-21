using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.GetByAdvertId
{
    /// <summary>
    /// Запрос для получения комментариев опубликованного объявления по идентификатору объявления.
    /// </summary>
    /// <param name="Id">Идентификатор опубликованного объявления.</param>
    /// <param name="Page">Номер страницы.</param>
    /// <param name="PageSize">Размер страницы.</param>
    public record GetCommentsByAdvertIdQuery(Guid AdvertId, int Page, int PageSize) : IQuery<PaginationCollection<CommentItem>>;
}

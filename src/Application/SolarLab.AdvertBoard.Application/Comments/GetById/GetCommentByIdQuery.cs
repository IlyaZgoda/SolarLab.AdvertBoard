using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.GetById
{
    /// <summary>
    /// Запрос для получения комментария по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор комментария.</param>
    public record GetCommentByIdQuery(Guid Id) : IQuery<CommentResponse>;
}

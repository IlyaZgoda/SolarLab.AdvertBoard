using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Create
{
    /// <summary>
    /// Команда для создания черновика объявления.
    /// </summary>
    /// <param name="AdvertId">Идентификатор объявления.</param>
    /// <param name="Text">Текст комментария.</param>
    public record CreateCommentCommand(Guid AdvertId, string Text) : ICommand<CommentIdResponse>;
}

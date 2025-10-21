using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Comments.Update
{
    /// <summary>
    /// Команда для обновления комментария по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор комментария.</param>
    /// <param name="Text">Новый текст комментария.</param>

    public record UpdateCommentCommand(Guid Id, string Text) : ICommand;
}

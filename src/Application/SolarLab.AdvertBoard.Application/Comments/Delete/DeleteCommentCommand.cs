using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Comments.Delete
{
    /// <summary>
    /// Команда для удаления комментария по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор комментария.</param>
    public record DeleteCommentCommand(Guid Id) : ICommand;
}

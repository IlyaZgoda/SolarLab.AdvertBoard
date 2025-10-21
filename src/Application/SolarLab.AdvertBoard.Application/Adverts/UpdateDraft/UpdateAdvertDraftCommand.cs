using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.UpdateDraft
{
    /// <summary>
    /// Команда для обновления черновика объявления.
    /// </summary>
    /// <param name="DraftId">Идентификатор черновика.</param>
    /// <param name="CategoryId">Новая категория (опционально).</param>
    /// <param name="Title">Новый заголовок (опционально).</param>
    /// <param name="Description">Новое описание (опционально).</param>
    /// <param name="Price">Новая цена (опционально).</param>
    public record UpdateAdvertDraftCommand(Guid DraftId, Guid? CategoryId, string? Title, string? Description, decimal? Price) : ICommand;
}

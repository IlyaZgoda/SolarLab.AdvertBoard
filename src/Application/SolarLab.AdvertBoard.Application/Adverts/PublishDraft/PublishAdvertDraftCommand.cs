using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.PublishDraft
{
    /// <summary>
    /// Команда для публикации черновика объявления.
    /// </summary>
    /// <param name="Id">Идентификатор черновика.</param>
    public record PublishAdvertDraftCommand(Guid Id) : ICommand;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.DeleteDraft
{
    /// <summary>
    /// Команда для удаления черновика объявления.
    /// </summary>
    /// <param name="Id">Идентификатор черновика.</param>
    public record DeleteAdvertDraftCommand(Guid Id) : ICommand;
}

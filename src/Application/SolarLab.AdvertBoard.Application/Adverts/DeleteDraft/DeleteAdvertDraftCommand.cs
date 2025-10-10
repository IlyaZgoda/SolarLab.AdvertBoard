using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.DeleteDraft
{
    public record DeleteAdvertDraftCommand(Guid Id) : ICommand;
}

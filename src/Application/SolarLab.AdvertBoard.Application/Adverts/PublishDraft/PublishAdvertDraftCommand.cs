using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.PublishDraft
{
    public record PublishAdvertDraftCommand(Guid Id) : ICommand;
}

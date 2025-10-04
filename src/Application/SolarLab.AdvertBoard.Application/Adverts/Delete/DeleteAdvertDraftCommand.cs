using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.Delete
{
    public record DeleteAdvertDraftCommand(Guid Id) : ICommand;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.CreateDraft
{
    public record CreateAdvertDraftCommand(Guid CategoryId, string Title, string Description, decimal Price) : ICommand<AdvertIdResponse>;
}

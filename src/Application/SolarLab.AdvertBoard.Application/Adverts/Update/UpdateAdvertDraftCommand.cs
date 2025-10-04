using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.Update
{
    public record UpdateAdvertDraftCommand(Guid DraftId, Guid? CategoryId, string? Title, string? Description, decimal? Price) : ICommand;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Images.Delete
{
    public record DeleteImageCommand(Guid AdvertId, Guid Id) : ICommand;
}

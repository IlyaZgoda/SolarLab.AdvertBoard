using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.DeletePublished
{
    public record DeletePublishedAdvertCommand(Guid Id) : ICommand;
}

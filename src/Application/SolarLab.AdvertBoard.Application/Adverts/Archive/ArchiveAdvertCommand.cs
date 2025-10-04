using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.Archive
{
    public record ArchiveAdvertCommand(Guid Id) : ICommand;
}

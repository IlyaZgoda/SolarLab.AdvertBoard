using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Adverts.DeletePublished
{
    /// <summary>
    /// Команда для удаления опубликованного объявления.
    /// </summary>
    /// <param name="Id">Идентификатор объявления.</param>
    public record DeletePublishedAdvertCommand(Guid Id) : ICommand;
}

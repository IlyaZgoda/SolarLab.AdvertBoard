using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Images.Delete
{
    /// <summary>
    /// Команда для удаления изображения по идентификатору.
    /// </summary>
    /// <param name="AdvertId">Идентфикатор объявления, содержащего удаляемое изображение.</param>
    /// <param name="Id">Идентификатор изображения.</param>
    public record DeleteImageCommand(Guid AdvertId, Guid Id) : ICommand;
}

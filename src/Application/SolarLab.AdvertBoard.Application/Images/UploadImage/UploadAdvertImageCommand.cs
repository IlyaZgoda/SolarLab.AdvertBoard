using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Images;

namespace SolarLab.AdvertBoard.Application.Images.UploadImage
{
    /// <summary>
    /// Команда для загрузки изображения.
    /// </summary>
    /// <param name="AdvertId">Идентификатор объявления, для которого загружается изображение.</param>
    /// <param name="FileName">Имя изображения.</param>
    /// <param name="ContentType">MIME-тип содержимого изображения.</param>
    /// <param name="Content">Бинарное содержимое изображения.</param>
    public record UploadAdvertImageCommand(Guid AdvertId, string FileName, string ContentType, byte[] Content) : ICommand<ImageIdResponse>;
}

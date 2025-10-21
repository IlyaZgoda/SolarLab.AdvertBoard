using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Images;

namespace SolarLab.AdvertBoard.Application.Images.GetById
{
    /// <summary>
    /// Запрос для получения изображения по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор изображения.</param>
    public record GetImageByIdQuery(Guid Id) : IQuery<ImageResponse>;
}

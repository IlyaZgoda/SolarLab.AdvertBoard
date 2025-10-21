using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    /// <summary>
    /// Провайдер для чтения данных изображений в read-слое (CQRS).
    /// </summary>
    public interface IImageReadProvider
    {
        /// <summary>
        /// Получает изображение по идентификатору с проверкой прав доступа.
        /// </summary>
        /// <param name="id">Идентификатор изображения.</param>
        /// <param name="identityId">Идентификатор пользователя для проверки прав доступа.</param>
        /// <returns>Данные изображения или None если не найдено или доступ запрещен.</returns>
        Task<Maybe<ImageResponse>> GetImageById(AdvertImageId id, string identityId);
    }
}

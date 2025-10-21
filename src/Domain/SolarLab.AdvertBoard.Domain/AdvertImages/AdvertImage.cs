using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    /// <summary>
    /// Представляет изображение объявления как сущность доменной модели.
    /// </summary>
    public class AdvertImage
    {
        /// <summary>
        /// Идентификатор изображения.
        /// </summary>
        public AdvertImageId Id { get; init; } = null!;

        /// <summary>
        /// Идентификатор объявления, к которому прикреплено изображение.
        /// </summary>
        public AdvertId AdvertId { get; init; } = null!;

        /// <summary>
        /// Имя файла изображения.
        /// </summary>
        public ImageFileName FileName { get; init; } = null!;

        /// <summary>
        /// MIME-тип содержимого изображения.
        /// </summary>
        public ImageContentType ContentType { get; init; } = null!;

        /// <summary>
        /// Бинарное содержимое изображения.
        /// </summary>
        public ImageContent Content { get; init; } = null!;

        /// <summary>
        /// Дата и время создания изображения.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Создает новое изображение объявления.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления.</param>
        /// <param name="fileName">Имя файла изображения.</param>
        /// <param name="contentType">MIME-тип содержимого.</param>
        /// <param name="content">Бинарное содержимое изображения.</param>
        /// <returns>Новый экземпляр изображения объявления.</returns>
        public static AdvertImage Create(AdvertId advertId, ImageFileName fileName, ImageContentType contentType, ImageContent content) =>
            new()
            {
                Id = new AdvertImageId(Guid.NewGuid()),
                AdvertId = advertId,
                FileName = fileName,
                ContentType = contentType,
                Content = content,
                CreatedAt = DateTime.UtcNow,
            };
    }
}

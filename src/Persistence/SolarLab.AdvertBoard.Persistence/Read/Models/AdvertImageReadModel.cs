using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    /// <summary>
    /// Read модель для изображения объявления.
    /// </summary>
    public class AdvertImageReadModel : IAdvertImageReadModel
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public Guid AdvertId { get; }

        public AdvertReadModel Advert { get; } = null!;

        /// <inheritdoc/>
        IAdvertReadModel IAdvertImageReadModel.Advert => Advert;

        /// <inheritdoc/>
        public string FileName { get; } = null!;

        /// <inheritdoc/>
        public string ContentType { get; } = null!;

        /// <inheritdoc/>
        public byte[] Content { get; } = null!;

        /// <inheritdoc/>
        public DateTime CreatedAt { get; }
    }

}

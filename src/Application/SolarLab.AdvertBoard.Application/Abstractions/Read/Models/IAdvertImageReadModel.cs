namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    /// <summary>
    /// Read-модель для изображения объявления.
    /// </summary>
    /// <remarks>
    /// Используется для представления данных в read-слое (CQRS).
    /// Содержит все необходимые данные для отображения изображения.
    /// </remarks>
    public interface IAdvertImageReadModel
    {
        /// <summary>
        /// Идентификатор изображения.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор объявления, к которому относится изображение.
        /// </summary>
        public Guid AdvertId { get; }

        /// <summary>
        /// Read-модель объявления, к которому относится изображение.
        /// </summary>
        public IAdvertReadModel Advert { get; }

        /// <summary>
        /// Имя файла изображения.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// MIME-тип содержимого изображения.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// Бинарное содержимое изображения.
        /// </summary>
        public byte[] Content { get; }

        /// <summary>
        /// Дата и время создания изображения.
        /// </summary>
        public DateTime CreatedAt { get; }
    }
}

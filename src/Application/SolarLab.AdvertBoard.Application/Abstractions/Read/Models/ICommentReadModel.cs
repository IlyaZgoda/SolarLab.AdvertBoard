namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    /// <summary>
    /// Read-модель для комментария.
    /// </summary>
    /// <remarks>
    /// Используется для представления данных в read-слое (CQRS).
    /// Содержит данные комментария и ссылки на связанные сущности.
    /// </remarks>
    public interface ICommentReadModel
    {
        /// <summary>
        /// Идентификатор комментария.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор объявления, к которому относится комментарий.
        /// </summary>
        public Guid AdvertId { get; }

        /// <summary>
        /// Read-модель объявления, к которому относится комментарий.
        /// </summary>
        public IAdvertReadModel Advert { get; }

        /// <summary>
        /// Идентификатор автора комментария.
        /// </summary>
        public Guid AuthorId { get; }

        /// <summary>
        /// Read-модель автора комментария.
        /// </summary>
        public IUserReadModel Author { get; }

        /// <summary>
        /// Текст комментария.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Дата и время создания комментария.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Дата и время последнего обновления комментария.
        /// </summary>
        public DateTime? UpdatedAt { get; }
    }
}

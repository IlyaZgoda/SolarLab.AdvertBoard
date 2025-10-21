using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    /// <summary>
    /// Read-модель для объявления.
    /// </summary>
    /// <remarks>
    /// Используется для представления данных в read-слое (CQRS).
    /// Содержит все необходимые данные для отображения объявления, включая связанные сущности.
    /// </remarks>
    public interface IAdvertReadModel
    {
        /// <summary>
        /// Идентификатор объявления.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор автора объявления.
        /// </summary>
        public Guid AuthorId { get; }

        /// <summary>
        /// Read-модель автора объявления.
        /// </summary>
        public IUserReadModel Author { get; }

        /// <summary>
        /// Идентификатор категории объявления.
        /// </summary>
        public Guid CategoryId { get; }

        /// <summary>
        /// Read-модель категории объявления.
        /// </summary>
        public ICategoryReadModel Category { get; }

        /// <summary>
        /// Заголовок объявления.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Описание объявления.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Цена в объявлении.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// Статус объявления.
        /// </summary>
        public AdvertStatus Status { get; }

        /// <summary>
        /// Дата и время создания объявления.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Дата и время публикации объявления.
        /// </summary>
        public DateTime? PublishedAt { get; }

        /// <summary>
        /// Дата и время последнего обновления объявления.
        /// </summary>
        public DateTime? UpdatedAt { get; }

        /// <summary>
        /// Коллекция изображений объявления.
        /// </summary>
        public IReadOnlyList<IAdvertImageReadModel> Images { get; }

        /// <summary>
        /// Коллекция комментариев к объявлению.
        /// </summary>
        public IReadOnlyList<ICommentReadModel> Comments { get; }
    }
}

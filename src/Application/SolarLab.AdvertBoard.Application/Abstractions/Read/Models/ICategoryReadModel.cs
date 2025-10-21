namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    /// <summary>
    /// Read-модель для категории.
    /// </summary>
    /// <remarks>
    /// Используется для представления данных в read-слое (CQRS).
    /// Поддерживает иерархическую структуру категорий.
    /// </remarks>
    public interface ICategoryReadModel
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор родительской категории.
        /// </summary>
        public Guid? ParentId { get; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Коллекция дочерних категорий.
        /// </summary>
        public IReadOnlyList<ICategoryReadModel> Childrens { get; }
    }
}

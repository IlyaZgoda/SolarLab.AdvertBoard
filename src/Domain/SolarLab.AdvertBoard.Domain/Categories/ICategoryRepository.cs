using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    /// <summary>
    /// Репозиторий для работы с категориями.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Добавляет новую категорию.
        /// </summary>
        /// <param name="category">Категория для добавления.</param>
        void Add(Category category);

        /// <summary>
        /// Получает категорию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <returns>Категория если найдена, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<Category>> GetByIdAsync(CategoryId id);

        /// <summary>
        /// Получает все категории.
        /// </summary>
        /// <returns>Коллекция всех категорий.</returns>
        Task<IReadOnlyList<Category>> GetAllAsync();
    }
}

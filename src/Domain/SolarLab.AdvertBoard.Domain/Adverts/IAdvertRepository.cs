using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Репозиторий для работы с объявлениями.
    /// </summary>
    public interface IAdvertRepository
    {
        /// <summary>
        /// Получает объявление по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объявления.</param>
        /// <returns>Объявление если найдено, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<Advert>> GetByIdAsync(AdvertId id);

        /// <summary>
        /// Получает объявление по спецификации.
        /// </summary>
        /// <param name="specification">Спецификация для фильтрации.</param>
        /// <returns>Объявление если найдено, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<Advert>> GetBySpecificationAsync(Specification<Advert> specification);

        /// <summary>
        /// Добавляет новое объявление.
        /// </summary>
        /// <param name="advert">Объявление для добавления.</param>
        void Add(Advert advert);

        /// <summary>
        /// Обновляет существующее объявление.
        /// </summary>
        /// <param name="advert">Объявление для обновления.</param>
        void Update(Advert advert);

        /// <summary>
        /// Удаляет объявление.
        /// </summary>
        /// <param name="advert">Объявление для удаления.</param>
        void Delete(Advert advert);
    }
}

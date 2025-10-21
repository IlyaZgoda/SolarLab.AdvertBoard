using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    /// <summary>
    /// Провайдер для чтения данных объявлений в read-слое (CQRS).
    /// </summary>
    /// <remarks>
    /// Предоставляет методы для получения различных представлений объявлений
    /// с поддержкой пагинации, фильтрации и спецификаций.
    /// </remarks>
    public interface IAdvertReadProvider
    {
        /// <summary>
        /// Получает детали черновика объявления по идентификатору с использованием спецификации.
        /// </summary>
        /// <param name="spec">Спецификация для фильтрации и включения связанных данных.</param>
        /// <returns>Детали черновика объявления или None если не найден.</returns>
        Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(Specification<IAdvertReadModel> spec);

        /// <summary>
        /// Получает пагинированный список черновиков объявлений пользователя.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="spec">Спецификация для фильтрации.</param>
        /// <returns>Пагинированная коллекция черновиков объявлений.</returns>
        Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(int page, int pageSize, Specification<IAdvertReadModel> spec);

        /// <summary>
        /// Получает пагинированный список опубликованных объявлений пользователя.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="spec">Спецификация для фильтрации.</param>
        /// <returns>Пагинированная коллекция опубликованных объявлений.</returns>
        Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(int page, int pageSize, Specification<IAdvertReadModel> spec);

        /// <summary>
        /// Получает детали опубликованного объявления по идентификатору с использованием спецификации.
        /// </summary>
        /// <param name="spec">Спецификация для фильтрации и включения связанных данных.</param>
        /// <returns>Детали опубликованного объявления или None если не найден.</returns>
        Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(Specification<IAdvertReadModel> spec);

        /// <summary>
        /// Получает пагинированный список опубликованных объявлений по фильтру.
        /// </summary>
        /// <param name="filter">Фильтр для поиска объявлений.</param>
        /// <returns>Пагинированная коллекция опубликованных объявлений.</returns>
        Task<PaginationCollection<PublishedAdvertItem>> GetPublishedAdvertsByFilterAsync(AdvertFilterRequest filter);
    }
}

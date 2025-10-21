using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    /// <summary>
    /// Провайдер для чтения данных комментариев в read-слое (CQRS).
    /// </summary>
    public interface ICommentReadProvider
    {
        /// <summary>
        /// Получает пагинированный список комментариев для объявления по идентификатору.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированная коллекция комментариев.</returns>
        Task<PaginationCollection<CommentItem>> GetCommentsByIdAsync(Guid advertId, int page, int pageSize);
    }
}

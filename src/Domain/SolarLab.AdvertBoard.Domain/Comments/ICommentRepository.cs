using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    /// <summary>
    /// Репозиторий для работы с комментариями.
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Получает комментарий по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор комментария.</param>
        /// <returns>Комментарий если найден, иначе <see cref="Maybe{T}.None"/>.</returns>
        Task<Maybe<Comment>> GetByIdAsync(CommentId id);

        /// <summary>
        /// Добавляет новый комментарий.
        /// </summary>
        /// <param name="comment">Комментарий для добавления.</param>
        void Add(Comment comment);

        /// <summary>
        /// Обновляет существующий комментарий.
        /// </summary>
        /// <param name="comment">Комментарий для обновления.</param>
        void Update(Comment comment);

        /// <summary>
        /// Удаляет комментарий.
        /// </summary>
        /// <param name="comment">Комментарий для удаления.</param>
        void Delete(Comment comment);
    }
}

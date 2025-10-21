using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Read;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Builders
{
    /// <summary>
    /// Билдер запросов для комментариев.
    /// </summary>
    /// <param name="context">Контекст (для чтения) базы данных.</param>
    public class CommentQueryBuilder(ReadDbContext context)
    {
        private IQueryable<CommentReadModel> _query = context.Comments;

        /// <summary>
        /// Включает связанные данные объявления в запрос.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public CommentQueryBuilder WithAdvert()
        {
            _query.Include(x => x.Advert);
            return this;
        }

        /// <summary>
        /// Включает связанные данные автора в запрос.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public CommentQueryBuilder WithAuthor()
        {
            _query.Include(x => x.Author);
            return this;
        }

        /// <summary>
        /// Фильтрует только по комментариям опубликованных объявлений.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public CommentQueryBuilder WhereAdvertPublished()
        {
            _query = _query.Where(x => x.Advert.Status == AdvertStatus.Published);
            return this;
        }

        /// <summary>
        /// Фильтрует по идентификатору объявления.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public CommentQueryBuilder WhereAdvertId(Guid advertId)
        {
            _query = _query.Where(x => x.AdvertId == advertId);
            return this;
        }

        /// <summary>
        /// Возвращает построенный запрос.
        /// </summary>
        /// <returns>Построенный запрос к базе данных.</returns>
        public IQueryable<CommentReadModel> Build() => _query;
    }
}

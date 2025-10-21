using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Read;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Builders
{
    /// <summary>
    /// Билдер запросов для объявлений.
    /// </summary>
    /// <param name="context">Контекст (для чтения) базы данных.</param>
    public class AdvertQueryBuilder(ReadDbContext context)
    {
        private IQueryable<AdvertReadModel> _query = context.Adverts;

        /// <summary>
        /// Включает связанные данные категории в запрос.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder WithCategory()
        {
            _query.Include(x => x.Category);
            return this;
        }

        /// <summary>
        /// Включает связанные данные автора в запрос.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder WithAuthor()
        {
            _query.Include(x => x.Author);
            return this;
        }

        /// <summary>
        /// Фильтрует только опубликованные объявления.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder WherePublished()
        {
            _query = _query.Where(x => x.Status == AdvertStatus.Published);
            return this;
        }

        /// <summary>
        /// Фильтрует объявления по категории.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder FilterByCategory(Guid? categoryId)
        {
            if (categoryId.HasValue)
                _query = _query.Where(x => x.CategoryId == categoryId.Value);
            return this;
        }

        /// <summary>
        /// Фильтрует объявления по автору.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder FilterByAuthor(Guid? authorId)
        {
            if (authorId.HasValue)
                _query = _query.Where(x => x.AuthorId == authorId.Value);
            return this;
        }

        /// <summary>
        /// Фильтрует объявления по ценовому диапозону.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder FilterByPriceRange(decimal? min, decimal? max)
        {
            if (min.HasValue)
                _query = _query.Where(x => x.Price >= min.Value);
            if (max.HasValue)
                _query = _query.Where(x => x.Price <= max.Value);
            return this;
        }

        /// <summary>
        /// Включает связанные данные автора в запрос.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder Search(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                _query = _query.Where(x =>
                    x.Title.Contains(text) ||
                    x.Description.Contains(text));
            return this;
        }

        /// <summary>
        /// Выполняет поиск объявлений по тексту в заголовке и описании.
        /// </summary>
        /// <returns>Текущий экземпляр билдера.</returns>
        public AdvertQueryBuilder Sort(string? sortBy, bool desc)
        {
            _query = sortBy?.ToLower() switch
            {
                "price" => desc ? _query.OrderByDescending(x => x.Price)
                                : _query.OrderBy(x => x.Price),
                "title" => desc ? _query.OrderByDescending(x => x.Title)
                                : _query.OrderBy(x => x.Title),
                "createdat" => desc ? _query.OrderByDescending(x => x.CreatedAt)
                                    : _query.OrderBy(x => x.CreatedAt),
                _ => _query.OrderByDescending(x => x.PublishedAt)
            };
            return this;
        }

        /// <summary>
        /// Возвращает построенный запрос.
        /// </summary>
        /// <returns>Построенный запрос к базе данных.</returns>
        public IQueryable<AdvertReadModel> Build() => _query;
    }
}

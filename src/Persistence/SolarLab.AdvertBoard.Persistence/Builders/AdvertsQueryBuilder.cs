using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Read;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Builders
{
    public class AdvertQueryBuilder(ReadDbContext context)
    {
        private IQueryable<AdvertReadModel> _query = context.Adverts;


        public AdvertQueryBuilder WithCategory()
        {
            _query.Include(x => x.Category);
            return this;
        }

        public AdvertQueryBuilder WithAuthor()
        {
            _query.Include(x => x.Author);
            return this;
        }

        public AdvertQueryBuilder WherePublished()
        {
            _query = _query.Where(x => x.Status == AdvertStatus.Published);
            return this;
        }

        public AdvertQueryBuilder FilterByCategory(Guid? categoryId)
        {
            if (categoryId.HasValue)
                _query = _query.Where(x => x.CategoryId == categoryId.Value);
            return this;
        }

        public AdvertQueryBuilder FilterByAuthor(Guid? authorId)
        {
            if (authorId.HasValue)
                _query = _query.Where(x => x.AuthorId == authorId.Value);
            return this;
        }

        public AdvertQueryBuilder FilterByPriceRange(decimal? min, decimal? max)
        {
            if (min.HasValue)
                _query = _query.Where(x => x.Price >= min.Value);
            if (max.HasValue)
                _query = _query.Where(x => x.Price <= max.Value);
            return this;
        }

        public AdvertQueryBuilder Search(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                _query = _query.Where(x =>
                    x.Title.Contains(text) ||
                    x.Description.Contains(text));
            return this;
        }

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

        public IQueryable<AdvertReadModel> Build() => _query;
    }
}

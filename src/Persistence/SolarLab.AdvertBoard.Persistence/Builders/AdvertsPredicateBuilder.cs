using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Adverts.Builder;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence.Builders
{
    public record AdvertView(Advert Advert, User User, Category Category);
    public class AdvertQueryBuilder
    {
        private IQueryable<AdvertView> _query;

        public AdvertQueryBuilder(ApplicationDbContext context)
        {
            _query = from advert in context.Adverts.AsNoTracking()
                     join category in context.Categories.AsNoTracking() on advert.CategoryId equals category.Id
                     join user in context.AppUsers.AsNoTracking() on advert.AuthorId equals user.Id
                     where advert.Status == AdvertStatus.Published
                     select new AdvertView(advert, user, category);
        }

        public AdvertQueryBuilder FilterByCategory(Guid? categoryId)
        {
            if (categoryId.HasValue)
                _query = _query.Where(x => x.Advert.CategoryId == categoryId.Value);
            return this;
        }

        public AdvertQueryBuilder FilterByAuthor(Guid? authorId)
        {
            if (authorId.HasValue)
                _query = _query.Where(x => x.Advert.AuthorId == authorId.Value);
            return this;
        }

        public AdvertQueryBuilder FilterByPriceRange(decimal? min, decimal? max)
        {
            if (min.HasValue)
                _query = _query.Where(x => (decimal)x.Advert.Price >= min.Value);
            if (max.HasValue)
                _query = _query.Where(x => (decimal)x.Advert.Price <= max.Value);
            return this;
        }

        public AdvertQueryBuilder Search(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                _query = _query.Where(x =>
                    x.Advert.Title.Value.Contains(text) ||
                    x.Advert.Description.Value.Contains(text));
            return this;
        }

        public AdvertQueryBuilder Sort(string? sortBy, bool desc)
        {
            _query = sortBy?.ToLower() switch
            {
                "price" => desc ? _query.OrderByDescending(x => x.Advert.Price)
                                : _query.OrderBy(x => x.Advert.Price),
                "title" => desc ? _query.OrderByDescending(x => x.Advert.Title.Value)
                                : _query.OrderBy(x => x.Advert.Title.Value),
                "createdat" => desc ? _query.OrderByDescending(x => x.Advert.CreatedAt)
                                    : _query.OrderBy(x => x.Advert.CreatedAt),
                _ => _query.OrderByDescending(x => x.Advert.PublishedAt)
            };
            return this;
        }

        public IQueryable<PublishedAdvertItem> Build() =>
            _query.Select(q => new PublishedAdvertItem(
                q.Advert.Id,
                q.Advert.Title.Value,
                q.Advert.Description.Value,
                q.Advert.Price.Value,
                q.Advert.CategoryId,
                q.Category.Title.Value,
                q.Advert.AuthorId,
                q.User.FullName,
                q.Advert.PublishedAt));
    }
}

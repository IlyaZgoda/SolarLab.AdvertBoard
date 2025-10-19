using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Persistence.Read;
using SolarLab.AdvertBoard.Persistence.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Builders
{
    public class CommentQueryBuilder(ReadDbContext context)
    {
        private IQueryable<CommentReadModel> _query = context.Comments;


        public CommentQueryBuilder WithAdvert()
        {
            _query.Include(x => x.Advert);
            return this;
        }

        public CommentQueryBuilder WithAuthor()
        {
            _query.Include(x => x.Author);
            return this;
        }

        public CommentQueryBuilder WhereAdvertPublished()
        {
            _query = _query.Where(x => x.Advert.Status == AdvertStatus.Published);
            return this;
        }
        public CommentQueryBuilder WhereAdvertId(Guid advertId)
        {
            _query = _query.Where(x => x.AdvertId == advertId);
            return this;
        }

        public IQueryable<CommentReadModel> Build() => _query;
    }
}

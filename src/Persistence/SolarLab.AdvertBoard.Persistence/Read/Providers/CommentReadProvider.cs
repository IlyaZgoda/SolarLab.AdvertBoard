using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Persistence.Builders;
using SolarLab.AdvertBoard.Persistence.Extensions;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    public class CommentReadProvider(ReadDbContext context) : ICommentReadProvider
    {
        public async Task<PaginationCollection<CommentItem>> GetCommentsByIdAsync(Guid advertId, int page, int pageSize)
        {
            var queryBuilder = new CommentQueryBuilder(context);

            return await queryBuilder
                .WithAdvert()
                .WithAuthor()
                .WhereAdvertId(advertId)
                .WhereAdvertPublished()
                .Build()
                .OrderByDescending(c => c.CreatedAt)
                .ToCommentItem()
                .ToPagedAsync(page, pageSize);
        } 
    }
}

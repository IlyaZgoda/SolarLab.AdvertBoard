using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Persistence.Builders;
using SolarLab.AdvertBoard.Persistence.Extensions;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    /// <summary>
    /// Провайдер для чтения данных комментариев.
    /// </summary>
    /// <param name="context">Контекст (для чтения) базы данных.</param>
    public class CommentReadProvider(ReadDbContext context) : ICommentReadProvider
    {
        /// <inheritdoc/>
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

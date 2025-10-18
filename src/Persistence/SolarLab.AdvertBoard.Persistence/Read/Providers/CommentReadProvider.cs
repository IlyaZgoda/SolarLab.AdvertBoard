using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Persistence.Extensions;

namespace SolarLab.AdvertBoard.Persistence.Read.Providers
{
    public class CommentReadProvider(ApplicationDbContext context) : ICommentReadProvider
    {
        public async Task<PaginationCollection<CommentItem>> GetCommentsByIdAsync(Guid advertId, int page, int pageSize) =>
            await (from comment in context.Comments.AsNoTracking()
                          join advert in context.Adverts.AsNoTracking()
                          on comment.AdvertId equals advert.Id
                          join user in context.AppUsers.AsNoTracking()
                          on comment.AuthorId equals user.Id
                          where advert.Id == advertId
                          orderby comment.CreatedAt descending
                          select new CommentItem(
                              comment.Id,
                              comment.AdvertId,
                              comment.AuthorId,
                              user.FullName,
                              comment.Text.Value,
                              comment.CreatedAt,
                              comment.UpdatedAt)).ToPagedAsync(page, pageSize);
    }
}

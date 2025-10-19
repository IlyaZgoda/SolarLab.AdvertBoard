using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    public interface ICommentReadProvider
    {
        Task<PaginationCollection<CommentItem>> GetCommentsByIdAsync(Guid advertId, int page, int pageSize);
    }
}

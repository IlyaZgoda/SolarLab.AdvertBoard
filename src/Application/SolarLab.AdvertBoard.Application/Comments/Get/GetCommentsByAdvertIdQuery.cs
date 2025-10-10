using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Get
{
    public record GetCommentsByAdvertIdQuery(Guid AdvertId, int Page, int PageSize) : IQuery<PaginationCollection<CommentItem>>;
}

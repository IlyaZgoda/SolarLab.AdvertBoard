using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.GetById
{
    public record GetCommentByIdQuery(Guid Id) : IQuery<CommentResponse>;
}

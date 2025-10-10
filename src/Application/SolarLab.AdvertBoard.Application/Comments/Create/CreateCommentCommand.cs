using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Create
{
    public record CreateCommentCommand(Guid AdvertId, string Text) : ICommand<CommentIdResponse>;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Comments.Delete
{
    public record DeleteCommentCommand(Guid Id) : ICommand;
}

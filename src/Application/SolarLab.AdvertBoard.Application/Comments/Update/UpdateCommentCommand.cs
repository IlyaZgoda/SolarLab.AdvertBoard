using SolarLab.AdvertBoard.Application.Abstractions.Messaging;

namespace SolarLab.AdvertBoard.Application.Comments.Update
{
    public record UpdateCommentCommand(Guid Id, string Text) : ICommand;
}

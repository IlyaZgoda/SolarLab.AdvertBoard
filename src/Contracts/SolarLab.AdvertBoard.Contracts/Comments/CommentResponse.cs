namespace SolarLab.AdvertBoard.Contracts.Comments
{
    public record CommentResponse(Guid Id, Guid AdvertId, Guid AuthorId, string Text, DateTime CreatedAt, DateTime? UpdatedAt);
}

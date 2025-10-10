namespace SolarLab.AdvertBoard.Contracts.Comments
{
    public record CommentItem(Guid Id, Guid AdvertId, Guid AuthorId, string FullName, string Text, DateTime CreatedAt, DateTime? UpdatedAt);
}

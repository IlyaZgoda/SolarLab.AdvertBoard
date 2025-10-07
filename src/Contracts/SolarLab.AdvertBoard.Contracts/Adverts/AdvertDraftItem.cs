namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record AdvertDraftItem(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        Guid CategoryId,
        string CategoryTitle,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        Guid AuthorId);
}

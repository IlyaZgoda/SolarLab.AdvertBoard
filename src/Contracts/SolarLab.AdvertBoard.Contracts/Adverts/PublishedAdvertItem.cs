namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record PublishedAdvertItem(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        Guid CategoryId,
        string CategoryName,
        Guid AuthorId,
        string AuthorName,
        DateTime? PublishedAt);
}

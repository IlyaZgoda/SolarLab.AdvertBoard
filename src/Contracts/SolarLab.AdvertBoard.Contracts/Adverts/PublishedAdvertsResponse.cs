namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record PublishedAdvertsResponse(List<PublishedAdvertItem> PublishedAdverts, int Page, int PageSize, int TotalCount, int TotalPages);

    public record PublishedAdvertItem(
        Guid Id,
        string Title,
        string Description,
        Guid CategoryId,
        string CategoryName,
        Guid AuthorId,
        string AuthorName,
        DateTime? PublishedAt);
}

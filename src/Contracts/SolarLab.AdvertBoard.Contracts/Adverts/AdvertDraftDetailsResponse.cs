namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record AdvertDraftDetailsResponse(
        Guid Id,
        string Title, 
        string Description, 
        decimal Price, 
        Guid CategoryId, 
        string CategoryTitle, 
        string Status, 
        DateTime CreatedAt, 
        DateTime? UpdatedAt, 
        Guid AuthorId);

    public record GetPublishedAdvertsByFilterRequest(int? Page, int? PageSize);
    public record GetUserAdvertDraftsRequest(int? Page, int? PageSize);
    public record AdvertFilter();

    public record AdvertDraftsResponse(List<AdvertDraftItem> PublishedAdverts, int Page, int PageSize, int TotalCount, int TotalPages);

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

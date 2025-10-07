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
}

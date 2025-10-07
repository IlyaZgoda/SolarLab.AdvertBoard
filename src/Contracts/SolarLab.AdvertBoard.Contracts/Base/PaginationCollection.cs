namespace SolarLab.AdvertBoard.Contracts.Base
{
    public record PaginationCollection<TItem>(
        IReadOnlyCollection<TItem> Items, 
        int Page, 
        int PageSize, 
        int TotalCount, 
        int TotalPages);
}

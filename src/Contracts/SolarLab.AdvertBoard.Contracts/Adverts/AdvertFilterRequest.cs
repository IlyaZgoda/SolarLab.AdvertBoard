using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record AdvertFilterRequest : IPagination
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? SortBy { get; init; }
        public bool SortDescending { get; init; } = true;

        public Guid? CategoryId { get; init; }
        public Guid? AuthorId { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? SearchText { get; init; }
    }
}

namespace SolarLab.AdvertBoard.Contracts.Base
{
    public interface IPagination
    {
        int Page { get; init; }
        int PageSize { get; init; }
    }
}

namespace SolarLab.AdvertBoard.Contracts.Adverts
{
    public record GetUserPublishedAdvertsRequest(int Page = 1, int PageSize = 20);
}

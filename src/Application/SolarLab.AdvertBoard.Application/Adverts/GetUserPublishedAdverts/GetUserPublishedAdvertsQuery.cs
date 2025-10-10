using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserPublishedAdverts
{
    public record GetUserPublishedAdvertsQuery(int Page = 1, int PageSize = 20) : IQuery<PaginationCollection<PublishedAdvertItem>>;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetUserPublishedAdvertsQuery(int? Page, int? PageSize) : IQuery<PublishedAdvertsResponse>;
}

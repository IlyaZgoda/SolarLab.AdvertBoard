using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetPublishedAdvertsByFilterQuery(int? Count, int? PageSize) : IQuery<PublishedAdvertsResponse>;
}

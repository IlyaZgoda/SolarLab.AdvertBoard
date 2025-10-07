using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetPublishedAdvertsByFilterQuery(AdvertFilterRequest Filter) : IQuery<PaginationCollection<PublishedAdvertItem>>;
}

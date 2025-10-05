using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetPublishedAdvertDetailsByIdQuery(Guid Id) : IQuery<PublishedAdvertDetailsResponse>;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetAdvertDraftByIdQuery(Guid Id) : IQuery<AdvertDraftResponse>;
}

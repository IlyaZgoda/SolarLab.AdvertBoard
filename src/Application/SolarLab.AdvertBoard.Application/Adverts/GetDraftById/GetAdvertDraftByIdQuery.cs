using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.GetDraftById
{
    public record GetAdvertDraftByIdQuery(Guid Id) : IQuery<AdvertDraftDetailsResponse>;
}

using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;

namespace SolarLab.AdvertBoard.Application.Adverts.Get
{
    public record GetUserAdvertDraftsQuery(int Page = 1, int PageSize = 20) : IQuery<PaginationCollection<AdvertDraftItem>>;
}

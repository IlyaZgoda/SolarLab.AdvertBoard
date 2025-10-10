using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Application.Abstractions.ReadProviders
{
    public interface IAdvertReadProvider
    {
        Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(AdvertId id);
        Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(string identityId, int page, int pageSize);
        Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(string identityId, int page, int pageSize);
        Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(AdvertId id);
        Task<PaginationCollection<PublishedAdvertItem>> GetPublishedAdvertsByFilterAsync(AdvertFilterRequest filter);
    }
}

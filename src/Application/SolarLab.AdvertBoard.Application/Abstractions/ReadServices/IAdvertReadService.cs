using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Application.Abstractions.ReadServices
{
    public interface IAdvertReadService
    {
        Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(AdvertId id);
        Task<Maybe<AdvertDraftsResponse>> GetUserAdvertDrafts(string identityId, int Page, int PageSize);
        Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(AdvertId id);
        Task<Maybe<PublishedAdvertsResponse>> GetPublishedAdvertsByFilterAsync(int page, int pageSize);
    }
}

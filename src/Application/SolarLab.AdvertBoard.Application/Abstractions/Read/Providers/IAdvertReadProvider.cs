using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    public interface IAdvertReadProvider
    {
        Task<Maybe<AdvertDraftDetailsResponse>> GetAdvertDraftDetailsByIdAsync(Specification<IAdvertReadModel> spec);
        Task<PaginationCollection<AdvertDraftItem>> GetUserAdvertDrafts(int page, int pageSize, Specification<IAdvertReadModel> spec);
        Task<PaginationCollection<PublishedAdvertItem>> GetUserPublishedAdverts(int page, int pageSize, Specification<IAdvertReadModel> spec);
        Task<Maybe<PublishedAdvertDetailsResponse>> GetPublishedAdvertDetailsByIdAsync(Specification<IAdvertReadModel> spec);
        Task<PaginationCollection<PublishedAdvertItem>> GetPublishedAdvertsByFilterAsync(AdvertFilterRequest filter);
    }
}

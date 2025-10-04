using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Application.Abstractions.ReadServices
{
    public interface IAdvertReadService
    {
        Task<Maybe<AdvertDraftResponse>> GetAdvertDraftDetailsByIdAsync(AdvertId id);
    }
}

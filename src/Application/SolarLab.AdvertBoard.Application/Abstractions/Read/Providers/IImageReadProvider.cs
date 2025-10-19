using SolarLab.AdvertBoard.Contracts.Images;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Providers
{
    public interface IImageReadProvider
    {
        Task<Maybe<ImageResponse>> GetImageById(AdvertImageId id, string identityId);
    }
}

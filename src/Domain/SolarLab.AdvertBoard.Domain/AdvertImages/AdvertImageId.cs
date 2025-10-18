using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    public record AdvertImageId(Guid Id) : StronglyTypedId(Id);
}

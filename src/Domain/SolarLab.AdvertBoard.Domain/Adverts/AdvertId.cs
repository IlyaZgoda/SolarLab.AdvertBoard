using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public record AdvertId(Guid Id) : StronglyTypedId(Id);
}

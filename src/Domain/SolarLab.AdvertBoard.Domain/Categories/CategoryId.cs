using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    public record CategoryId(Guid Id) : StronglyTypedId(Id);
}

using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public record UserId(Guid Id) : StronglyTypedId(Id);
}

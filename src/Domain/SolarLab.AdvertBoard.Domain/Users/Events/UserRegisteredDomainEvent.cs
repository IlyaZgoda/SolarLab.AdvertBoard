using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users.Events
{
    public record UserRegisteredDomainEvent(UserId UserId, string IdentityId) : IDomainEvent;
}

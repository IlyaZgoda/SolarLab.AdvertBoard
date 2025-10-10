using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Adverts.Events
{
    public record PublishedAdvertDeletedDomainEvent(Guid Id) : IDomainEvent;
}

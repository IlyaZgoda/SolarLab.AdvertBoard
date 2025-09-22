using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Persistence
{
    public class UnitOfWork(ApplicationDbContext context, IMediator mediator) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var result = await context.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }

        private async Task PublishDomainEventsAsync()
        {
            var aggregateRoots = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Count != 0)
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
                .ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}

using MediatR;
using SolarLab.AdvertBoard.Domain.Users.Events;

namespace SolarLab.AdvertBoard.Application.Register
{
    public class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("EventHandler works!!!");
            await Task.CompletedTask;
        }
    }
}

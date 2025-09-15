using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.AdvertBoard.Application.Register;
using SolarLab.AdvertBoard.Domain.Users.Events;
using System.Reflection;

namespace SolarLab.AdvertBoard.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

           // services.AddTransient<INotificationHandler<UserRegisteredDomainEvent>, UserRegisteredDomainEventHandler>();

            return services;

        }
    }
}

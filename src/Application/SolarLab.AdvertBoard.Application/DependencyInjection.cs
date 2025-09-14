using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SolarLab.AdvertBoard.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}

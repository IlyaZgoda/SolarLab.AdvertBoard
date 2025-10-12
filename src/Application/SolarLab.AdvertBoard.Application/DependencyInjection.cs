using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace SolarLab.AdvertBoard.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;

        }
    }
}

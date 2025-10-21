using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace SolarLab.AdvertBoard.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Добавляет в IOC контейнер сервисы слоя Application.
        /// </summary>
        /// <param name="services">Коллекция дескрипторов сервисов.</param>
        /// <returns>Коллекцию дескрипторов сервисов</returns>
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

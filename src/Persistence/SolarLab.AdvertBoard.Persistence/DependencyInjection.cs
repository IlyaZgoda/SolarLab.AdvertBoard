using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.Persistence.Repositories;
using SolarLab.AdvertBoard.Persistence.Seeders;

namespace SolarLab.AdvertBoard.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<CategorySeeder>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserRepository, UserRepostory>();
            services.AddScoped<ICategoryRepository, CategoryRepostory>();
            services.AddScoped<IAdvertRepository, AdvertRepository>();
            
            return services;
        }
    }
}

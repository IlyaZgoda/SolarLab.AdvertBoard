using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Infrastructure.Authentication;
using System.Text;

namespace SolarLab.AdvertBoard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            
            var secret = configuration["Jwt:Secret"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var expirationInMinutes = configuration.GetValue<int>("Jwt:ExpirationInMinutes");

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}


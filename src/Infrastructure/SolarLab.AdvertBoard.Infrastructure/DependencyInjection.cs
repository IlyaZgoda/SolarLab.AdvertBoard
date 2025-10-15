using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Emails;
using SolarLab.AdvertBoard.Application.Abstractions.Links;
using SolarLab.AdvertBoard.Application.Abstractions.Notifications;
using SolarLab.AdvertBoard.Infrastructure.Authentication;
using SolarLab.AdvertBoard.Infrastructure.Emails;
using SolarLab.AdvertBoard.Infrastructure.Links;
using SolarLab.AdvertBoard.Infrastructure.Notifications;
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
            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
            services.Configure<UriGeneratorOptions>(configuration.GetSection("UriGenerator"));
            
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = tokenValidationParameters;
              });

            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddScoped<IUserManagerProvider, UserManagerProvider>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();  
            services.AddScoped<IEmailNotificationSender, EmailNotificationSender>();  
            services.AddScoped<IUriGenerator, UriGenerator>();  
            services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

            return services;
        }
    }
}


using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using SolarLab.AdvertBoard.Api.Extensions;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Api.Middleware;
using SolarLab.AdvertBoard.Application;
using SolarLab.AdvertBoard.Infrastructure;
using SolarLab.AdvertBoard.Persistence;

namespace SolarLab.AdvertBoard.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRazorPages();

            builder.Services
                .AddApplication()
                .AddPersistence(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

            builder.Services.AddSwaggerGenWithAuth();
            builder.Services.AddSingleton<ErrorToHttpMapper>();
            builder.Services.AddSingleton<ResultErrorHandler>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseExceptionHandler();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(swaggerUiOptions =>
                    swaggerUiOptions.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "SolarLab.AdvertBoard API"));

                await app.ApplyMigrationsAsync();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            await app.RunAsync();
        }
    }
}

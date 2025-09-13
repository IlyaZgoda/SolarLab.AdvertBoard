using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using SolarLab.AdvertBoard.Api.Extensions;
using SolarLab.AdvertBoard.Infrastructure;
using SolarLab.AdvertBoard.Persistence;

namespace SolarLab.AdvertBoard.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services
                .AddPersistence(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

            builder.Services.AddSwaggerGenWithAuth();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(swaggerUiOptions =>
                    swaggerUiOptions.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "SolarLab.AdvertBoard API"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}

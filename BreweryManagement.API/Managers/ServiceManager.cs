using BreweryManagement.Domain.Repositories;
using BreweryManagement.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BreweryManagement.API.Managers
{
    public static class ServiceManager
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Add swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Brewery Management API",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add DbContext
            services.AddDbContext<BeerRepository>(options => { });

            // Add services
            services.AddTransient<IBeerService, BeerService>();
        }
    }
}

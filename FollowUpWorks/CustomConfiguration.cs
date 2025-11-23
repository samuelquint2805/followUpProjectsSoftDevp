using FollowUpWorks.services.Abstractions;
using FollowUpWorks.services.Implementations;

namespace FollowUpWorks
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            //cache
            builder.Services.AddMemoryCache();  
            // Auto mapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            //self services
            AddServices(builder);
            return builder;
        }
        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CustomQuerableOperationsService>();
            builder.Services.AddScoped<IJSONRecipes,JSONRecipes>();
            builder.Services.AddScoped<IJSONReservation, JSONReservation>();
        }

        public static WebApplication WebAppCustomConfiguration(this WebApplication app)
        {
            return app;
        }
        }
}

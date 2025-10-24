using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BackCoding.Challenge.WebApi.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BackCodingDbContext>();

            try
            {
                dbContext.Database.Migrate();
                Log.Information("Migraciones aplicadas correctamente al iniciar la aplicación.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error aplicando migraciones al iniciar la aplicación.");
            }
        }
    }
}

using BackCoding.Challenge.Infrastructure.Extensions;
using BackCoding.Challenge.WebApi.Extensions;
using BackCoding.Challenge.WebApi.Middlewares;
using Serilog;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // AWS Logging + HealthCheck
        builder.AddAwsMonitoring();

        // Infraestructura
        builder.Services.AddInfrastructure(builder.Configuration);

        // Application Layer (MediatR, AutoMapper, Validators)
        builder.Services.AddApplicationServices();

        // JWT + Roles
        builder.Services.AddJwtAuthentication(builder.Configuration);

        // Swagger + JWT
        builder.Services.AddSwaggerWithJwt();

        // Controllers y CORS
        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", p =>
                p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        var app = builder.Build();

        // Middlewares
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        // Swagger
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Endpoints
        app.MapControllers();
        app.MapHealthChecks("/health");

        // Aplicar migraciones automáticamente (solo una vez al arrancar)
        app.ApplyMigrations();
        app.Services.EnsureFunctionsCreated();
        app.Services.SeedBaseData();

        app.MapGet("/version", () => Results.Ok(new { version = "v1.1 - redeploy test OK" }));
        // Run
        app.Run();
    }
}
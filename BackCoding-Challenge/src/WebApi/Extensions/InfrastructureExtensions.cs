using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Infrastructure.Persistence.Context;
using BackCoding.Challenge.Infrastructure.Reports;
using BackCoding.Challenge.Infrastructure.Repositories;
using BackCoding.Challenge.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace BackCoding.Challenge.WebApi.Extensions
{
    public static class InfrastructureExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<BackCodingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositorios y UoW
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Reportes
            services.AddScoped<IClientProductsReportRepository, ClientProductsReportRepository>();

            // Servicios de notificación
            services.AddScoped<EmailNotificationService>();
            services.AddScoped<SmsNotificationService>();
            services.AddScoped<INotificationStrategyResolver, NotificationStrategyResolver>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}

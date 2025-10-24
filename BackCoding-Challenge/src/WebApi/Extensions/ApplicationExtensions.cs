using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace BackCoding.Challenge.WebApi.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var appAssembly = Assembly.Load("BackCoding.Challenge.Application");

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(appAssembly);

            // AutoMapper
            services.AddAutoMapper(cfg => { }, appAssembly);

            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(appAssembly));
        }
    }
}

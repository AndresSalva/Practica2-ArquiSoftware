using Microsoft.Extensions.DependencyInjection;
using ServiceDiscipline.Domain.Ports;
using ServiceDiscipline.Infrastructure.Persistence;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Application.Services;

namespace ServiceDiscipline.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDisciplineModule(this IServiceCollection services)
        {
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            return services;
        }
    }
}

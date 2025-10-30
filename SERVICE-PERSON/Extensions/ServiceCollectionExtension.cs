using Microsoft.Extensions.DependencyInjection;
using ServicePerson.Domain.Ports;
using ServicePerson.Infraestructure.Persistence;
using ServicePerson.Application.Interfaces;
using ServicePerson.Application.Services;

namespace ServicePerson.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersonModule(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonService, PersonService>();
            return services;
        }
    }
}

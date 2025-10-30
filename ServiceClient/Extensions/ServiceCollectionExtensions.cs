using Microsoft.Extensions.DependencyInjection;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Persistence;
using ServiceClient.Application.Interfaces;
using ServiceClient.Application.Services;

namespace ServiceClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientModule(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();
            return services;
        }
    }
}

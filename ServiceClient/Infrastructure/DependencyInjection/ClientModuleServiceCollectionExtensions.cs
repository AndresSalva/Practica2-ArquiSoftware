using Microsoft.Extensions.DependencyInjection;
using ServiceClient.Application.Interfaces;
using ServiceClient.Application.Services;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Persistence;
using ServiceClient.Infrastructure.Providers;
using System.Data;

namespace ServiceClient.Infrastructure.DependencyInjection
{
    public static class ClientModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddClientModule(this IServiceCollection services, Func<IServiceProvider, string> connectionStringFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionStringFactory, nameof(connectionStringFactory));

            services.AddSingleton<IClientConnectionProvider>(sp =>
            {
                var connectionString = connectionStringFactory(sp);
                return new DelegatedClientConnectionProvider(connectionString);
            });

            return services.AddClientCore();
        }

        public static IServiceCollection AddClientModule<TProvider>(this IServiceCollection services)
            where TProvider : class, IClientConnectionProvider
        {
            services.AddSingleton<IClientConnectionProvider, TProvider>();
            return services.AddClientCore();
        }

        private static IServiceCollection AddClientCore(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IDetailClientService, DetailClientService>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IDetailClientRepository, DetailClientRepository>();

            return services;
        }
        
        private sealed class DelegatedClientConnectionProvider : IClientConnectionProvider
        {
            private readonly string _connectionString;

            public DelegatedClientConnectionProvider(string connectionString)
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentException("La cadena de conexión no puede ser nula ni estar vacía.", nameof(connectionString));
                }
                _connectionString = connectionString;
            }

            public IDbConnection CreateConnection() => new Npgsql.NpgsqlConnection(_connectionString);
        }
    }
}
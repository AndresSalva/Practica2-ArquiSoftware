// Ruta: ServiceClient/Infrastructure/DependencyInjection/ClientModuleServiceCollectionExtensions.cs

using Microsoft.Extensions.DependencyInjection;
using ServiceClient.Application.Interfaces;
using ServiceClient.Application.Services;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Persistence;
using ServiceClient.Infrastructure.Providers;
using System;
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

        /// <summary>
        /// Contiene el registro de todos los servicios y repositorios principales del módulo.
        /// </summary>
        private static IServiceCollection AddClientCore(this IServiceCollection services)
        {
            // Application Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IDetailClientService, DetailClientService>();
            // --- ¡AÑADIR ESTA LÍNEA! ---
            services.AddScoped<IUserService, UserService>(); // Asumiendo que la clase se llama UserService

            // Infrastructure Repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IDetailClientRepository, DetailClientRepository>();
            // --- ¡AÑADIR ESTA LÍNEA! ---
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        /// <summary>
        /// Implementación interna del proveedor de conexión.
        /// </summary>
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
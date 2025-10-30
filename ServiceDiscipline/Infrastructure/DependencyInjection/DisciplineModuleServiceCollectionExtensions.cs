using Microsoft.Extensions.DependencyInjection;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Application.Services;
using ServiceDiscipline.Domain.Ports;
using ServiceDiscipline.Infrastructure.Persistence;
using ServiceDiscipline.Infrastructure.Provider;

namespace ServiceDiscipline.Infrastructure.DependencyInjection
{
    public static class DisciplineModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddDisciplineModule<TProvider>(this IServiceCollection services)
            where TProvider : class, IDisciplineConnectionProvider
        {
            // Registra el provider que la aplicación principal nos da.
            services.AddSingleton<IDisciplineConnectionProvider, TProvider>();

            return services.AddDisciplineCore();
        }

        // Permite registrar el módulo pasando una función que sabe cómo obtener la connection string.
        public static IServiceCollection AddDisciplineModule(this IServiceCollection services, Func<IServiceProvider, string> connectionStringFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionStringFactory, nameof(connectionStringFactory));

            services.AddSingleton<IDisciplineConnectionProvider>(sp =>
            {
                var connectionString = connectionStringFactory(sp);
                return new DelegatedDisciplineConnectionProvider(connectionString);
            });

            return services.AddDisciplineCore();
        }


        private static IServiceCollection AddDisciplineCore(this IServiceCollection services)
        {
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<IDisciplineService, DisciplineService>();


            return services;
        }

        private sealed class DelegatedDisciplineConnectionProvider : IDisciplineConnectionProvider
        {
            private readonly string _connectionString;

            public DelegatedDisciplineConnectionProvider(string connectionString)
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentException("La cadena de conexión no puede ser nula ni estar vacía.", nameof(connectionString));
                }
                _connectionString = connectionString;
            }

            public string GetConnectionString() => _connectionString;
        }
    }
}
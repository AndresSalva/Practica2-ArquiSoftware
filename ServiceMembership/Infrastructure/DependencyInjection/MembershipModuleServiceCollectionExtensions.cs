using Microsoft.Extensions.DependencyInjection;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Application.Services;
using ServiceMembership.Domain.Ports;
using ServiceMembership.Infrastructure.Persistence;
using ServiceMembership.Infrastructure.Providers;

namespace ServiceMembership.Infrastructure.DependencyInjection;

public static class MembershipModuleServiceCollectionExtensions
{
    public static IServiceCollection AddMembershipModule(this IServiceCollection services, Func<IServiceProvider, string> connectionStringFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionStringFactory, nameof(connectionStringFactory));

        services.AddSingleton<IMembershipConnectionProvider>(sp =>
        {
            var connectionString = connectionStringFactory(sp);
            return new DelegatedMembershipConnectionProvider(connectionString);
        });

        return services.AddMembershipCore();
    }

    public static IServiceCollection AddMembershipModule<TProvider>(this IServiceCollection services)
        where TProvider : class, IMembershipConnectionProvider
    {
        services.AddSingleton<IMembershipConnectionProvider, TProvider>();
        return services.AddMembershipCore();
    }

    private static IServiceCollection AddMembershipCore(this IServiceCollection services)
    {
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IDetailUserRepository, DetailUserRepository>();
        services.AddScoped<IDetailUserService, DetailUserService>();
        return services;
    }

    private sealed class DelegatedMembershipConnectionProvider : IMembershipConnectionProvider
    {
        private readonly string _connectionString;

        public DelegatedMembershipConnectionProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public string GetConnectionString() => _connectionString;
    }
}

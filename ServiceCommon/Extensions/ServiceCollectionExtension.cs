using Microsoft.Extensions.DependencyInjection;
using ServiceCommon.Infrastructure.Services;
using ServiceCommon.Domain.Ports;

namespace ServiceCommon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonModule(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<ParameterProtector>();
            services.AddSingleton<IRemoteLogger, RemoteLogger>();
            services.AddTransient<IEmailSender, EmailService>();
            return services;
        }
    }
}

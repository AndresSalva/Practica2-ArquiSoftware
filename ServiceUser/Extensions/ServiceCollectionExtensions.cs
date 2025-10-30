using Microsoft.Extensions.DependencyInjection;
using ServiceUser.Domain.Ports;
using ServiceUser.Infrastructure.Persistence;
using ServiceUser.Application.Interfaces;
using ServiceUser.Application.Services;

namespace ServiceUser.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}

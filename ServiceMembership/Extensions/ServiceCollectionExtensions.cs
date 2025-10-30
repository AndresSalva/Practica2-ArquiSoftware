using Microsoft.Extensions.DependencyInjection;
using ServiceMembership.Domain.Ports;
using ServiceMembership.Infrastructure.Persistence;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Application.Services;

namespace ServiceMembership.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMembershipModule(this IServiceCollection services)
        {
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IDetailMembershipRepository, DetailMembershipRepository>();
            services.AddScoped<IDetailMembershipService, DetailMembershipService>();
            return services;
        }
    }
}

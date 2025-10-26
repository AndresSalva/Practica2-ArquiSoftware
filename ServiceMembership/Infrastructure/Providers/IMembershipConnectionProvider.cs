namespace ServiceMembership.Infrastructure.Providers;

public interface IMembershipConnectionProvider
{
    string GetConnectionString();
}

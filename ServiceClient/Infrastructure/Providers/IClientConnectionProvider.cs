using System.Data;

namespace ServiceClient.Infrastructure.Providers
{
    public interface IClientConnectionProvider
    {
        IDbConnection CreateConnection();
    }
}
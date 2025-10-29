using Microsoft.Extensions.Configuration;
using ServiceCommon.Domain.Ports;

namespace ServiceCommon.Infrastructure.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPostgresConnection()
        {
            var conn = _configuration.GetConnectionString("Postgres");
            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException("No se encontró la cadena de conexión 'Postgres'.");
            return conn;
        }
    }
}

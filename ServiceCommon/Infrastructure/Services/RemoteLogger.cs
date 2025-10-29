using Dapper;
using Npgsql;
using ServiceCommon.Domain.Entities;
using ServiceCommon.Domain.Ports;

namespace ServiceCommon.Infrastructure.Services
{
    public class RemoteLogger : IRemoteLogger
    {
        private readonly string _postgresConnectionString;
        public RemoteLogger(string postgresConnectionString)
        {
            _postgresConnectionString = postgresConnectionString;
        }
        
        private async Task<int> LogAsync(string level, string message, string userIdentifier)
        {
            if (string.IsNullOrEmpty(_postgresConnectionString))
            {
                Console.WriteLine("WARNING: Logger sin cadena de conexión. El Log fue descartado.");
                return 0;
            }

            try
            {
                using var conn = new NpgsqlConnection(_postgresConnectionString);
                var logEntry = new LogEntry
                {
                    CreatedAt = DateTime.UtcNow,
                    Level = level,
                    Message = message,
                    ClientIdentifier = userIdentifier ?? "Unknown"
                };

                const string sql = @"INSERT INTO logs 
                    (created_at, level, message, client_identifier) 
                    VALUES (@CreatedAt, @Level, @Message, @ClientIdentifier)";

                Task<int> result = conn.ExecuteAsync(sql, logEntry);
                return result.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar log remoto: {ex.Message}");
                return 0;
            }
        }

        public Task LogInfo(string message, string user) => LogAsync("Info", message, user);
        public Task LogWarning(string message, string user) => LogAsync("Warning", message, user);
        public Task LogError(string message, string user, Exception? ex = null)
        {
            var fullMessage = ex != null ? $"{message}. Excepción: {ex}" : message;
            return LogAsync("Error", fullMessage, user);
        }
    }
}

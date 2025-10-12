using GYMPT.Models;
using Microsoft.Extensions.Configuration; // <-- AÑADIR ESTE USING
using Npgsql; // <-- AÑADIR ESTE USING
using Dapper; // <-- AÑADIR ESTE USING
using System;
using System.Threading.Tasks;

namespace GYMPT.Services
{
    public sealed class RemoteLoggerSingleton
    {
        #region Singleton implementation

        private static RemoteLoggerSingleton _instance;
        private static readonly object _lock = new object();

        private string _postgresString;

        private RemoteLoggerSingleton() { }

        public static void Configure()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RemoteLoggerSingleton();
                        _instance._postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
                    }
                }
            }
        }

        public static RemoteLoggerSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("The RemoteLoggerSingleton has not been configured. Call Configure() in Program.cs.");
                }
                return _instance;
            }
        }

        #endregion

        #region Logger

        private async Task Log(string level, string message)
        {
            if (string.IsNullOrEmpty(_postgresString))
            {
                Console.WriteLine("WARNING: The logger does not have a connection string. The log will be lost.");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(_postgresString))
                {
                    var machineName = Environment.MachineName;
                    var logEntry = new LogEntry
                    {
                        CreatedAt = DateTime.UtcNow,
                        Level = level,
                        Message = message,
                        ClientIdentifier = machineName
                    };

                    var sql = "INSERT INTO logs (created_at, level, message, client_identifier) VALUES (@CreatedAt, @Level, @Message, @ClientIdentifier)";

                    await conn.ExecuteAsync(sql, logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FALLO EN EL LOGGER: No se pudo escribir en la base de datos PostgreSQL. Error: {ex.Message}");
            }
        }

        #endregion

        #region Public methods

        public async Task LogInfo(string message)
        {
            await Log("Info", message);
        }

        public async Task LogError(string message, Exception ex = null)
        {
            string fullMessage = ex != null ? $"{message}. Excepción: {ex.ToString()}" : message;
            await Log("Error", fullMessage);
        }

        public async Task LogWarning(string message)
        {
            await Log("Warning", message);
        }

        #endregion
    }
}
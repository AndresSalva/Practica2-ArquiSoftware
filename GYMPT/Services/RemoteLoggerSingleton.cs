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
        #region Implementación del Patrón Singleton

        private static RemoteLoggerSingleton _instance;
        private static readonly object _lock = new object();

        // El logger ahora guardará la cadena de conexión
        private string _postgresString;

        private RemoteLoggerSingleton() { }

        // El método de configuración ahora recibe IConfiguration para leer el appsettings.json
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
                    throw new InvalidOperationException("El RemoteLoggerSingleton no ha sido configurado. Llama a Configure() en Program.cs.");
                }
                return _instance;
            }
        }

        #endregion

        #region Lógica del Logger (Adaptada a PostgreSQL y Dapper)

        private async Task Log(string level, string message)
        {
            if (string.IsNullOrEmpty(_postgresString))
            {
                Console.WriteLine("ADVERTENCIA: El logger no tiene una cadena de conexión. El log se perderá.");
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

                    // Asume que tienes una tabla "logs"
                    var sql = "INSERT INTO logs (timestamp, level, message, client_identifier) VALUES (@Timestamp, @Level, @Message, @ClientIdentifier)";

                    // Dapper se encarga de la ejecución
                    await conn.ExecuteAsync(sql, logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FALLO EN EL LOGGER: No se pudo escribir en la base de datos PostgreSQL. Error: {ex.Message}");
            }
        }

        #endregion

        #region Métodos Públicos (Sin cambios)

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
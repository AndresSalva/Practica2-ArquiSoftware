using GYMPT.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GYMPT.Services
{

    public sealed class RemoteLoggerSingleton
    {
        #region Implementación del Patrón Singleton

        private static RemoteLoggerSingleton _instance;

        private static readonly object _lock = new object();

        private RemoteLoggerSingleton() { }
        public static RemoteLoggerSingleton Instance
        {
            get
            {

                if (_instance == null)
                {

                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RemoteLoggerSingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Inicialización y Lógica del Logger

        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task Log(string level, string message)
        {
            if (_serviceProvider == null)
            {
                Console.WriteLine("ADVERTENCIA: El logger remoto no ha sido inicializado. El log se perderá.");
                return;
            }

            try
            {

                using (var scope = _serviceProvider.CreateScope())
                {

                    var supabaseClient = scope.ServiceProvider.GetRequiredService<Supabase.Client>();

                    var machineName = Environment.MachineName;

                    var logEntry = new LogEntry
                    {
                        Timestamp = DateTime.UtcNow,
                        Level = level,
                        Message = message,
                        ClientIdentifier = machineName
                    };

                    await supabaseClient.From<LogEntry>().Insert(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FALLO EN EL LOGGER REMOTO: No se pudo enviar el log a Supabase. Error: {ex.Message}");
            }
        }

        #endregion

        #region Métodos Públicos de Logging (API del Logger)

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
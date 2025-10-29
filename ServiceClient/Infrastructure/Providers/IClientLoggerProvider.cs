// Ruta: ServiceClient/Infrastructure/Providers/IClientLoggerProvider.cs

using System;
using System.Threading.Tasks;

// --- ESTA LÍNEA ES LA CORRECCIÓN MÁS IMPORTANTE ---
// Asegúrate de que el namespace coincide con la estructura de carpetas.
namespace ServiceClient.Infrastructure.Providers
{
    // El resto de la declaración de la interfaz.
    // Asegúrate de que no haya un error de tipeo en el nombre de la interfaz.
    public interface IClientLoggerProvider
    {
        Task LogInfo(string message);
        Task LogError(string message, Exception ex = null);
        Task LogWarning(string message);
    }
}
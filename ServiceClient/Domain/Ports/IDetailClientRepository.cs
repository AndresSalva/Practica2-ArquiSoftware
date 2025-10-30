// Ruta: ServiceClient/Domain/Ports/IDetailClientRepository.cs

using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Domain.Ports
{
    public interface IDetailClientRepository
    {
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();

        // --- INICIO DE LA CORRECCIÓN ---
        Task<Client> FindByCiAsync(string ci);
        // --- FIN DE LA CORRECCIÓN ---
    }
}
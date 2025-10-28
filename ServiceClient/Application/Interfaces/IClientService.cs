using ServiceClient.Domain.Entities; // Asegúrate de que la entidad Client está aquí
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Interfaces
{
    public interface IClientService
    {
        // ---> ESTA ES LA FIRMA DEL CONTRATO <---
        Task<Client?> GetByIdAsync(int id);
        // ----------------------------------------

        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> CreateAsync(Client client);
        Task<Client?> UpdateAsync(Client client);
        Task<bool> DeleteByIdAsync(int id);
    }
}
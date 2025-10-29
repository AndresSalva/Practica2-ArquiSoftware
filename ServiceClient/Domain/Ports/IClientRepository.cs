using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Domain.Ports
{
    /// <summary>
    /// Define el contrato para el repositorio de clientes.
    /// Esta interfaz es el "puerto" que la capa de Aplicación usa para hablar con la Infraestructura.
    /// </summary>
    public interface IClientRepository
    {
        // --- ASEGÚRATE DE QUE ESTA FIRMA ESTÉ PRESENTE Y SEA PÚBLICA ---
        Task<Client?> GetByIdAsync(int id);

        Task<IEnumerable<Client>> GetAllAsync();

        Task<Client> CreateAsync(Client entity);

        Task<Client?> UpdateAsync(Client entity);

        Task<bool> DeleteByIdAsync(int id);
    }
}
using ServiceClient.Domain.Entities;

namespace ServiceClient.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client?> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> CreateAsync(Client client);
        Task<Client?> UpdateAsync(Client client);
        Task<bool> DeleteByIdAsync(int id);
    }
}
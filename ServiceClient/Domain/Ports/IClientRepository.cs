using ServiceClient.Domain.Entities;

namespace ServiceClient.Domain.Ports
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id);

        Task<IEnumerable<Client>> GetAllAsync();

        Task<Client> CreateAsync(Client entity);

        Task<Client?> UpdateAsync(Client entity);

        Task<bool> DeleteByIdAsync(int id);
    }
}
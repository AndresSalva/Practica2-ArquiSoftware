using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task CreateNewClient(Client newClient);
        Task UpdateClientData(Client clientToUpdate);
        Task<bool> DeleteClient(int id);
    }
}
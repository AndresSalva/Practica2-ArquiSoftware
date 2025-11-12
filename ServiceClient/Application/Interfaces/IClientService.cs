using ServiceClient.Application.Common;
using ServiceClient.Domain.Entities;

namespace ServiceClient.Application.Interfaces
{
    public interface IClientService
    {
        
        Task<Result<Client>> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task<Result<Client>> CreateNewClient(Client client);
        Task<Result<Client>> UpdateClient(Client client);
        Task<Result<bool>> DeleteClient(int id);

    }
}
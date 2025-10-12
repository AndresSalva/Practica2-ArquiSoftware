using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Task<Client> GetClientById(int id) => _clientRepository.GetByIdAsync(id);
        public Task<IEnumerable<Client>> GetAllClients() => _clientRepository.GetAllAsync();
        public Task CreateNewClient(Client newClient) => _clientRepository.CreateAsync(newClient);
        public Task UpdateClientData(Client clientToUpdate) => _clientRepository.UpdateAsync(clientToUpdate);
        public Task DeleteClient(int id) => _clientRepository.DeleteByIdAsync(id);
    }
}
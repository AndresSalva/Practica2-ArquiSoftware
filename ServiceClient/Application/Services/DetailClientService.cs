using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Services
{
    public class DetailClientService : IDetailClientService
    {
        private readonly IDetailClientRepository _detailClientRepository;

        public DetailClientService(IDetailClientRepository detailClientRepository)
        {
            _detailClientRepository = detailClientRepository;
        }

        public async Task<Client> GetClientByIdAsync(int clientId)
        {
            return await _detailClientRepository.GetByIdAsync(clientId);
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _detailClientRepository.GetAllAsync();
        }
    }
}
using Microsoft.Extensions.Logging;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities; // Asegúrate de tener este using
using ServiceClient.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Services
{
    public class ClientService : IClientService // <-- El error CS0535 ocurre en esta línea
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ---> ESTA ES LA IMPLEMENTACIÓN. DEBE COINCIDIR EXACTAMENTE CON EL CONTRATO <---
        public async Task<Client?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo cliente con Id {ClientId}", id);
            return await _clientRepository.GetByIdAsync(id);
        }
        // --------------------------------------------------------------------------

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los clientes");
            return await _clientRepository.GetAllAsync();
        }

        public async Task<Client> CreateAsync(Client client)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            _logger.LogInformation("Creando un nuevo cliente con nombre {ClientName}", client.Name);
            return await _clientRepository.CreateAsync(client);
        }

        public async Task<Client?> UpdateAsync(Client client)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            _logger.LogInformation("Actualizando cliente con Id {ClientId}", client.Id);
            return await _clientRepository.UpdateAsync(client);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            _logger.LogInformation("Eliminando cliente con Id {ClientId}", id);
            return await _clientRepository.DeleteByIdAsync(id);
        }
    }
}
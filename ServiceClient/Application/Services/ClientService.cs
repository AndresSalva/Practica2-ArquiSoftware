// Ruta: ServiceClient/Application/Services/ClientService.cs

using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        // Primero, se valida el objeto que llega desde la capa de presentación.
        var validationResult = ClientValidationRules.Validate(client);

        if (validationResult.IsFailure)
        {
            throw new ArgumentException(validationResult.Error);
        }

        // --- ¡ESTA ES LA LÍNEA DE LA SOLUCIÓN! ---
        // Se asigna la fecha de creación ANTES de pasarlo a la capa de persistencia.
        // Usar UtcNow es la mejor práctica para evitar problemas de zona horaria en el servidor.
        client.CreatedAt = DateTime.UtcNow;
        client.IsActive = true; // También es un buen lugar para establecer valores por defecto.
        // ------------------------------------------

        // Ahora el objeto 'client' se envía al repositorio con el valor de 'CreatedAt' ya establecido.
        return await _clientRepository.CreateAsync(client);
    }

    // --- El resto de los métodos no cambian ---
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            throw new ArgumentException(validationResult.Error);
        }
        return await _clientRepository.UpdateAsync(client);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await _clientRepository.DeleteByIdAsync(id);
    }
}
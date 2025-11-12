// Ruta: ServiceClient/Application/Services/ClientService.cs

using ServiceClient.Application.Common;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Domain.Rules;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<Client>> CreateNewClient(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            return Result<Client>.Failure(validationResult.Error);
        }

        client.CreatedAt = DateTime.UtcNow;
        client.IsActive = true;

        var createdClient = await _clientRepository.CreateAsync(client);
        return Result<Client>.Success(createdClient);
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Result<Client>> GetClientById(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client == null)
        {
            return Result<Client>.Failure($"No se encontró el cliente con ID {id}.");
        }

        return Result<Client>.Success(client);
    }

    public async Task<Result<Client>> UpdateClient(Client client)
    {
        var validationResult = ClientValidationRules.Validate(client);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        var updatedClient = await _clientRepository.UpdateAsync(client);

        if (updatedClient == null)
        {
            return Result<Client>.Failure($"No se encontró el cliente con ID {client.Id} para actualizar.");
        }

        return Result<Client>.Success(updatedClient);
    }

    public async Task<Result<bool>> DeleteClient(int id)
    {
        var success = await _clientRepository.DeleteByIdAsync(id);

        if (!success)
        {
            return Result<bool>.Failure($"No se pudo eliminar el cliente con ID {id} (probablemente no se encontró).");
        }

        return Result<bool>.Success(true);
    }
}
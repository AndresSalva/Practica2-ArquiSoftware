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

    public async Task<Result<Client>> CreateAsync(Client client)
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
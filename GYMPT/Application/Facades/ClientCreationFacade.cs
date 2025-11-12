using GYMPT.Application.DTO;
using ServiceClient.Application.Common;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
namespace GYMPT.Application.Facades
{
    public class ClientCreationFacade
    {
        private readonly IClientService _clientService;
        public ClientCreationFacade(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Result<Client>> CreateClientAsync(UserInputModel input)
        {
            try
            {
                // Validación de rol para asegurar que este método solo maneje clientes.
                if (input.Role != "Client")
                {
                    return Result<Client>.Failure("El rol especificado no es 'Cliente'.");
                }

                // 1. Mapear el input a la entidad Client
                var client = new Client
                {
                    Name = input.Name,
                    FirstLastname = input.FirstLastname,
                    SecondLastname = input.SecondLastname,
                    Ci = input.Ci,
                    DateBirth = input.DateBirth,
                    FitnessLevel = input.FitnessLevel,
                    EmergencyContactPhone = input.EmergencyContactPhone,
                    InitialWeightKg = input.InitialWeightKg,
                    CurrentWeightKg = input.CurrentWeightKg
                };

                // 2. Llamar al servicio específico de cliente.
                // Asumimos que _clientService.CreateNewClient ya devuelve un Result<Client>.
                var clientResult = await _clientService.CreateNewClient(client);

                // 3. Devolver el resultado directamente.
                // Si el servicio de cliente tuvo éxito, este resultado será exitoso.
                // Si falló (ej. validación), este resultado contendrá el error.
                return clientResult;
            }
            catch (Exception ex)
            {
                // Captura cualquier error inesperado (ej. problema de conexión a BD)
                return Result<Client>.Failure($"Ocurrió un error inesperado al crear el cliente: {ex.Message}");
            }
        }
        public async Task<Result<Client>> GetUserByIdAsync(int id)
        {
            // Simplemente delegamos la llamada y devolvemos el Result del servicio.
            return await _clientService.GetClientById(id);
        }

        public async Task<Result<Client>> UpdateUserAsync(Client updatedUser)
        {
            ArgumentNullException.ThrowIfNull(updatedUser, nameof(updatedUser));

            // Simplemente delegamos la llamada. El servicio se encargará de validar
            // y comprobar si el usuario existe antes de actualizar.
            return await _clientService.UpdateClient(updatedUser);
        }
    }
}

using GYMPT.Application.DTO;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Application.Common;

namespace GYMPT.Infrastructure.Facade
{
    public class UserCreationFacade
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly EmailService _emailService; // Asumiendo que este servicio no necesita refactorización por ahora

        public UserCreationFacade(
            IClientService clientService,
            IUserService userService,
            EmailService emailService)
        {
            _clientService = clientService;
            _userService = userService;
            _emailService = emailService;
        }

        // =====================================================
        // 🧩 CREAR USUARIO (Refactorizado con Patrón Result)
        // =====================================================
        public async Task<Result<User>> CreateUserAsync(UserInputModel input)
        {
            try
            {
                if (input.Role == "Client")
                {
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

                    var clientResult = await _clientService.CreateNewClient(client);
                    if (clientResult.IsFailure)
                    {
                        return Result<User>.Failure(clientResult.Error);
                    }

                    var createdUser = clientResult.Value;
                    return Result<User>.Success(new User { Name = createdUser.Name, FirstLastname = createdUser.FirstLastname });
                }
                else if (input.Role == "Instructor")
                {
                    var instructor = new User
                    {
                        Name = input.Name,
                        FirstLastname = input.FirstLastname,
                        SecondLastname = input.SecondLastname,
                        Ci = input.Ci,
                        DateBirth = input.DateBirth,
                        Role = "Instructor",
                        Email = input.Email,
                        Password = "gympt." + input.Ci,
                        Specialization = input.Specialization,
                        HireDate = input.HireDate,
                        MonthlySalary = input.MonthlySalary
                    };

                    // El servicio de usuario ahora devuelve un Result<User>
                    var userResult = await _userService.CreateUser(instructor);
                    if (userResult.IsFailure)
                    {
                        return userResult; // El error ya está en el formato correcto.
                    }

                    // Lógica de envío de correo (solo si la creación fue exitosa)
                    if (!string.IsNullOrWhiteSpace(input.Email))
                    {
                        // ... tu código de envío de correo ...
                    }

                    return userResult;
                }

                return Result<User>.Failure("El rol de usuario especificado no es válido.");
            }
            catch (Exception ex)
            {
                // Captura cualquier error inesperado (ej. problema de conexión a BD)
                return Result<User>.Failure($"Ocurrió un error inesperado en el sistema: {ex.Message}");
            }
        }

        // =====================================================
        // 🔍 OBTENER USUARIO POR ID (Refactorizado)
        // =====================================================
        public async Task<Result<User>> GetUserByIdAsync(int id)
        {
            // Simplemente delegamos la llamada y devolvemos el Result del servicio.
            return await _userService.GetUserById(id);
        }

        // =====================================================
        // 🔄 ACTUALIZAR USUARIO EXISTENTE (Refactorizado)
        // =====================================================
        public async Task<Result<User>> UpdateUserAsync(User updatedUser)
        {
            ArgumentNullException.ThrowIfNull(updatedUser, nameof(updatedUser));

            // Simplemente delegamos la llamada. El servicio se encargará de validar
            // y comprobar si el usuario existe antes de actualizar.
            return await _userService.UpdateUser(updatedUser);
        }
    }
}
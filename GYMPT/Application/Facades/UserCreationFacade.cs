using GYMPT.Application.DTO;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Application.Common;
using ServiceUser.Domain.Entities;
namespace GYMPT.Application.Facade
{
    public class UserCreationFacade
    {
        private readonly IUserService _userService;
        private readonly EmailService _emailService; // Asumiendo que este servicio no necesita refactorización por ahora

        public UserCreationFacade(
            IUserService userService,
            EmailService emailService)
        {
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
                // Validación de rol. Este método podría manejar 'Instructor', 'Admin', etc.
                if (input.Role != "Instructor")
                {
                    return Result<User>.Failure("El rol especificado no es 'Instructor'.");
                }

                // 1. Mapear el input a la entidad User
                var instructor = new User
                {
                    Name = input.Name,
                    FirstLastname = input.FirstLastname,
                    SecondLastname = input.SecondLastname,
                    Ci = input.Ci,
                    DateBirth = input.DateBirth,
                    Role = "Instructor",
                    Email = input.Email,
                    Password = "gympt." + input.Ci, // Generación de contraseña temporal
                    Specialization = input.Specialization,
                    HireDate = input.HireDate,
                    MonthlySalary = input.MonthlySalary
                };

                // 2. Llamar al servicio de usuario.
                var userResult = await _userService.CreateUser(instructor);

                // 3. Si la creación falla, devolver el error inmediatamente.
                if (userResult.IsFailure)
                {
                    return userResult;
                }

                // 4. Si la creación es exitosa, proceder con acciones adicionales (como enviar correo).
                if (!string.IsNullOrWhiteSpace(input.Email))
                {
                    // Aquí iría tu código para enviar el correo de bienvenida con la contraseña temporal.
                    // Ejemplo: await _emailService.SendWelcomeEmailAsync(userResult.Value);
                }

                // 5. Devolver el resultado exitoso.
                return userResult;
            }
            catch (Exception ex)
            {
                // Captura cualquier error inesperado
                return Result<User>.Failure($"Ocurrió un error inesperado al crear el usuario: {ex.Message}");
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
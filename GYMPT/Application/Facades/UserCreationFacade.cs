using GYMPT.Application.DTO;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;

namespace GYMPT.Infrastructure.Facade
{
    public class UserCreationFacade
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly EmailService _emailService;

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
        // 🧩 CREAR USUARIO (Instructor o Cliente)
        // =====================================================
        public async Task<bool> CreateUserAsync(UserInputModel input)
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

                await _clientService.CreateAsync(client);
                return true;
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
                    HireDate = input.HireDate ?? DateTime.MinValue,
                    MonthlySalary = input.MonthlySalary ?? 0m
                };

                await _userService.CreateUser(instructor);

                if (!string.IsNullOrWhiteSpace(input.Email))
                {
                    string subject = "Tu cuenta GYMPT fue creada";
                    string body = $@"
                        <h3>¡Bienvenido/a {input.Name}!</h3>
                        <p>Tu cuenta ha sido creada correctamente.</p>
                        <p><strong>Correo:</strong> {input.Email}</p>
                        <p><strong>Contraseña:</strong> gympt.{input.Ci}</p>
                        <p>Por seguridad, cambia tu contraseña al iniciar sesión.</p>
                        <hr>
                        <p>© GYMPT, 2025</p>
                    ";
                    // TODO
                    // await _emailService.SendEmailAsync(input.Email, subject, body);
                }

                return true;
            }

            return false;
        }

        // =====================================================
        // 🔍 OBTENER USUARIO POR ID
        // =====================================================
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userService.GetUserById(id);
        }

        // =====================================================
        // 🔄 ACTUALIZAR USUARIO EXISTENTE (Instructor)
        // =====================================================
        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            if (updatedUser == null)
                return false;

            var existingUser = await _userService.GetUserById(updatedUser.Id);
            if (existingUser == null)
                return false;

            // Actualizamos campos
            existingUser.Name = updatedUser.Name;
            existingUser.FirstLastname = updatedUser.FirstLastname;
            existingUser.SecondLastname = updatedUser.SecondLastname;
            existingUser.Ci = updatedUser.Ci;
            existingUser.DateBirth = updatedUser.DateBirth;
            existingUser.Specialization = updatedUser.Specialization;
            existingUser.HireDate = updatedUser.HireDate;
            existingUser.MonthlySalary = updatedUser.MonthlySalary;
            existingUser.Email = updatedUser.Email;

            await _userService.UpdateUser(existingUser);
            return true;
        }
    }
}

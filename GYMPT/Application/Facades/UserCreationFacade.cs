using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using GYMPT.Pages.Users;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using GYMPT.Application.DTO;

namespace GYMPT.Infrastructure.Facade
{
    public class UserCreationFacade
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly EmailService _emailService;

        public UserCreationFacade(
            IClientService clientService,
            IUserService userService,
            IPasswordHasher passwordHasher,
            EmailService emailService)
        {
            _clientService = clientService;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

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

                await _clientService.CreateNewClient(client);
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
                    Password = _passwordHasher.Hash("gympt." + input.Ci),
                    Specialization = input.Specialization,
                    HireDate = input.HireDate ?? DateTime.MinValue,
                    MonthlySalary = input.MonthlySalary ?? 0m
                };

                await _userService.CreateUser(instructor);

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

                await _emailService.SendEmailAsync(input.Email, subject, body);
                return true;
            }

            return false;
        }
    }
}

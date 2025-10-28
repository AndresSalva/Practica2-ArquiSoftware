using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceClient.Application.Interfaces; // <-- Apuntar a la nueva interfaz
using ServiceClient.Domain.Entities;      // <-- Apuntar a la nueva entidad
using GYMPT.Application.Interfaces;     // Mantenemos esto para IInstructorService (hasta que se module)
using GYMPT.Domain.Entities;            // Mantenemos esto para Instructor (hasta que se module)
using GYMPT.Infrastructure.Services;      // Mantenemos esto para los servicios de UI
using System.Threading.Tasks;             // Necesario para async/await

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class CreateModel : PageModel
    {
        // IClientService ahora viene del nuevo módulo
        private readonly IClientService _clientService;
        // IInstructorService todavía viene del proyecto antiguo
        private readonly IInstructorService _instructorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly EmailService _email;

        public CreateModel(IClientService clientService, IInstructorService instructorService, IPasswordHasher passwordHasher, EmailService email)
        {
            _clientService = clientService;
            _instructorService = instructorService;
            _passwordHasher = passwordHasher;
            _email = email;
        }

        [BindProperty]
        public UserInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.Role == "Client")
            {
                // La entidad 'Client' ahora viene del namespace ServiceClient.Domain.Entities
                var newClient = new Client
                {
                    Name = Input.Name,
                    FirstLastname = Input.FirstLastname,
                    SecondLastname = Input.SecondLastname,
                    Ci = Input.Ci,
                    DateBirth = Input.DateBirth,
                    Role = "Client",
                    FitnessLevel = string.IsNullOrWhiteSpace(Input.FitnessLevel) ? null : Input.FitnessLevel,
                    EmergencyContactPhone = string.IsNullOrWhiteSpace(Input.EmergencyContactPhone) ? null : Input.EmergencyContactPhone,
                    InitialWeightKg = Input.InitialWeightKg,
                    CurrentWeightKg = Input.CurrentWeightKg
                };

                // --- CAMBIO 2: Usar el nombre de método correcto del nuevo contrato ---
                await _clientService.CreateAsync(newClient);
                TempData["SuccessMessage"] = $"El cliente '{newClient.Name} {newClient.FirstLastname}' ha sido creado exitosamente.";
            }
            else if (Input.Role == "Instructor")
            {
                // La lógica del instructor no cambia porque aún no se ha movido a su propio módulo.
                var newInstructor = new Instructor
                {
                    Name = Input.Name,
                    FirstLastname = Input.FirstLastname,
                    SecondLastname = Input.SecondLastname,
                    Ci = Input.Ci,
                    DateBirth = Input.DateBirth,
                    Role = "Instructor",
                    Email = Input.Email,
                    Password = _passwordHasher.Hash("gympt." + Input.Ci),
                    Specialization = string.IsNullOrWhiteSpace(Input.Specialization) ? null : Input.Specialization,
                    HireDate = Input.HireDate ?? DateTime.MinValue,
                    MonthlySalary = Input.MonthlySalary ?? 0m
                };
                string subject = "Tu cuenta GYMPT fue creada";
                string body = $@"
                        <h3>¡Bienvenido/a {Input.Name}!</h3>
                        <p>Tu cuenta ha sido creada correctamente.</p>
                        <p><strong>Correo:</strong> {Input.Email}</p>
                        <p><strong>Contraseña:</strong> gympt.{Input.Ci}</p>
                        <p>Por seguridad, cambia tu contraseña al iniciar sesión.</p>
                        <hr>
                        <p>© GYMPT, 2025</p>
                    ";

                await _email.SendEmailAsync(Input.Email, subject, body);
                await _instructorService.CreateNewInstructor(newInstructor);
                TempData["SuccessMessage"] = $"El instructor '{newInstructor.Name} {newInstructor.FirstLastname}' ha sido creado exitosamente.";
            }

            return RedirectToPage("/Users/User");
        }
    }

    // El modelo de entrada no necesita cambios.
    public class UserInputModel
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string SecondLastname { get; set; }
        public string Ci { get; set; }
        public DateTime DateBirth { get; set; }

        // --- Campos de Cliente ---
        public string FitnessLevel { get; set; }
        public string EmergencyContactPhone { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }

        // --- Campos de Instructor ---
        public string Specialization { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Email { get; set; }
    }
}
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IUserService _instructorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly EmailService _email;

        public CreateModel(IClientService clientService, IUserService instructorService, IPasswordHasher passwordHasher, EmailService email)
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

                await _clientService.CreateNewClient(newClient);
                TempData["SuccessMessage"] = $"El cliente '{newClient.Name} {newClient.FirstLastname}' ha sido creado exitosamente.";
            }
            else if (Input.Role == "Instructor")
            {
                var newInstructor = new User
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
                    //rrth giwk oxmi pwiv
                    HireDate = Input.HireDate ?? DateTime.MinValue,
                    MonthlySalary = Input.MonthlySalary ?? 0m

                };
                string subject = "Tu cuenta GYMPT fue creada";
                string body = $@"
                        <h3>�Bienvenido/a {Input.Name}!</h3>
                        <p>Tu cuenta ha sido creada correctamente.</p>
                        <p><strong>Correo:</strong> {Input.Email}</p>
                        <p><strong>Contrase�a:</strong> gympt.{Input.Ci}</p>
                        <p>Por seguridad, cambia tu contrase�a al iniciar sesi�n.</p>
                        <hr>
                        <p>� GYMPT, 2025</p>
                    ";

                await _email.SendEmailAsync(Input.Email, subject, body);

                await _instructorService.CreateNewInstructor(newInstructor);
                TempData["SuccessMessage"] = $"El instructor '{newInstructor.Name} {newInstructor.FirstLastname}' ha sido creado exitosamente.";
            }

            return RedirectToPage("/Users/User");
        }
    }

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
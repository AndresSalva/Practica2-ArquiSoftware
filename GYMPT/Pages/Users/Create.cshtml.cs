// Ruta: GYMPT/Pages/Users/CreateModel.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using System.Threading.Tasks;
using System;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IInstructorService _instructorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailSender _email;

        public CreateModel(IClientService clientService, IInstructorService instructorService, IPasswordHasher passwordHasher, IEmailSender email)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.Role == "Client")
            {
                try
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
                        // Nota: CreatedAt aquí es null, lo cual es correcto para esta capa.
                    };

                    await _clientService.CreateAsync(newClient);
                    TempData["SuccessMessage"] = $"El cliente '{newClient.Name} {newClient.FirstLastname}' ha sido creado exitosamente.";
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }
            }
            else if (Input.Role == "Instructor")
            {
                // ... Lógica para crear instructor ...
            }

            return RedirectToPage("/Users/User");
        }
    }

    // Modelo de entrada para el formulario
    public class UserInputModel
    {
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? FirstLastname { get; set; }
        public string? SecondLastname { get; set; }
        public string? Ci { get; set; }
        public DateTime DateBirth { get; set; }
        public string? FitnessLevel { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }
        public string? Specialization { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Email { get; set; }
    }
}
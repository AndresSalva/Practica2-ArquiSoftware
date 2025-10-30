using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceCommon.Domain.Ports;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Pages.People
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientsService;
        private readonly IUserService _usersService;
        private readonly IEmailSender _emailService;

        public CreateModel(IClientService clientService, IUserService userService, IEmailSender emailService)
        {
            _clientsService = clientService;
            _usersService = userService;
            _emailService = emailService;
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
                        FitnessLevel = string.IsNullOrWhiteSpace(Input.FitnessLevel) ? null : Input.FitnessLevel,
                        EmergencyContactPhone = string.IsNullOrWhiteSpace(Input.EmergencyContactPhone) ? null : Input.EmergencyContactPhone,
                        InitialWeightKg = Input.InitialWeightKg,
                        CurrentWeightKg = Input.CurrentWeightKg
                    };

                    await _clientsService.CreateAsync(newClient);
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
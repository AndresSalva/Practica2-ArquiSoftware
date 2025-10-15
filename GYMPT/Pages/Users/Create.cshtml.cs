using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IInstructorService _instructorService;

        public CreateModel(IClientService clientService, IInstructorService instructorService)
        {
            _clientService = clientService;
            _instructorService = instructorService;
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
                var newInstructor = new Instructor
                {
                    Name = Input.Name,
                    FirstLastname = Input.FirstLastname,
                    SecondLastname = Input.SecondLastname,
                    Ci = Input.Ci,
                    DateBirth = Input.DateBirth,
                    Role = "Instructor",
                    Email = Input.Email,
                    Password = "",
                    Specialization = string.IsNullOrWhiteSpace(Input.Specialization) ? null : Input.Specialization,

                    HireDate = Input.HireDate ?? DateTime.MinValue, 
                    MonthlySalary = Input.MonthlySalary ?? 0m 
                    
                };

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
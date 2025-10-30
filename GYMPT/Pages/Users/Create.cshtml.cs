using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceUser.Domain.Entities;
using ServiceUser.Application.Interfaces;
using ServiceCommon.Domain.Ports;

namespace GYMPT.Pages.Users
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        private readonly IUserService _instructorService;
        private readonly IEmailSender _email;

        public CreateModel(IClientService clientService, IUserService instructorService, IEmailSender email)
        {
            _clientService = clientService;
            _instructorService = instructorService;
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
                        FitnessLevel = string.IsNullOrWhiteSpace(Input.FitnessLevel) ? null : Input.FitnessLevel,
                        EmergencyContactPhone = string.IsNullOrWhiteSpace(Input.EmergencyContactPhone) ? null : Input.EmergencyContactPhone,
                        InitialWeightKg = Input.InitialWeightKg,
                        CurrentWeightKg = Input.CurrentWeightKg
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
                try
                {
                    var newInstructor = new User
                    {
                        Name = Input.Name,
                        FirstLastname = Input.FirstLastname,
                        SecondLastname = Input.SecondLastname,
                        Ci = Input.Ci,
                        DateBirth = Input.DateBirth,
                        Role = Input.Role,
                        Specialization = Input.Specialization,
                        HireDate = Input.HireDate,
                        MonthlySalary = Input.MonthlySalary,
                        Email = Input.Email,
                    };

                    await _instructorService.CreateUser(newInstructor);
                    TempData["SuccessMessage"] = $"El instructor '{newInstructor.Name} {newInstructor.FirstLastname}' ha sido creado exitosamente.";
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }
            }

            return RedirectToPage("/Users/User");
        }
    }

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
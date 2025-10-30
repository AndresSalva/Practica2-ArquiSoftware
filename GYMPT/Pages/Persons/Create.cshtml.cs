using GYMPT.Application.DTO;
using GYMPT.Infrastructure.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Rules;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserCreationFacade _userCreationFacade;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserCreationFacade userCreationFacade, ILogger<CreateModel> logger)
        {
            _userCreationFacade = userCreationFacade;
            _logger = logger;
        }

        [BindProperty]
        public UserInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Input.Role?.Equals("Instructor", StringComparison.OrdinalIgnoreCase) == true)
            {
                var instructorUser = new User
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
                    Email = Input.Email
                };

                var validationResult = UserValidator.Validar(instructorUser);

                if (validationResult.IsFailure)
                {
                    var errors = validationResult.Error.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var error in errors)
                    {
                        _logger.LogWarning("Error de validación al crear Instructor: {Error}", error);

                        if (error.Contains("fecha de contratación", StringComparison.OrdinalIgnoreCase))
                            ModelState.AddModelError(nameof(Input.HireDate), error.Trim());
                        else if (error.Contains("18 años", StringComparison.OrdinalIgnoreCase) || error.Contains("nacimiento"))
                            ModelState.AddModelError(nameof(Input.DateBirth), error.Trim());
                        else if (error.Contains("CI", StringComparison.OrdinalIgnoreCase))
                            ModelState.AddModelError(nameof(Input.Ci), error.Trim());
                        else if (error.Contains("Especialización", StringComparison.OrdinalIgnoreCase))
                            ModelState.AddModelError(nameof(Input.Specialization), error.Trim());
                        else if (error.Contains("Sueldo", StringComparison.OrdinalIgnoreCase))
                            ModelState.AddModelError(nameof(Input.MonthlySalary), error.Trim());
                        else if (error.Contains("correo", StringComparison.OrdinalIgnoreCase))
                            ModelState.AddModelError(nameof(Input.Email), error.Trim());
                        else
                            ModelState.AddModelError(string.Empty, error.Trim());
                    }

                    return Page();
                 }
            }

            var result = await _userCreationFacade.CreateUserAsync(Input);

            if (result)
            {
                TempData["SuccessMessage"] = $"El usuario '{Input.Name} {Input.FirstLastname}' ha sido creado exitosamente.";
                _logger.LogInformation("Usuario creado: {Name} {Lastname}", Input.Name, Input.FirstLastname);
                return RedirectToPage("/Persons/Person");
            }
            else
            {
                var msg = $"No se pudo crear el usuario '{Input.Name} {Input.FirstLastname}'.";
                _logger.LogWarning(msg);
                ModelState.AddModelError(string.Empty, msg);
                return Page();
            }
        }
    }
}

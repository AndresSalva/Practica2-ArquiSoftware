using GYMPT.Infrastructure.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceCommon.Infrastructure.Services;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Rules;

namespace GYMPT.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserCreationFacade _userFacade;
        private readonly ParameterProtector _urlTokenSingleton;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public User Instructor { get; set; } = new();

        public EditModel(UserCreationFacade userFacade, ParameterProtector urlTokenSingleton, ILogger<EditModel> logger)
        {
            _userFacade = userFacade;
            _urlTokenSingleton = urlTokenSingleton;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Persons/Person");

            var idStr = _urlTokenSingleton.Unprotect(token);
            if (!int.TryParse(idStr, out var id))
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("/Persons/Person");
            }

            var instructor = await _userFacade.GetUserByIdAsync(id);
            if (instructor.IsFailure)
            {
                ModelState.AddModelError(string.Empty, instructor.Error);
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Persons/Person");
            }

            Instructor = instructor.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            //if (Instructor.Role?.Equals("Instructor", StringComparison.OrdinalIgnoreCase) == true)
            //{
            //    var validationResult = UserValidator.Validar(Instructor);

            //    if (validationResult.IsFailure)
            //    {
            //        var errors = validationResult.Error.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //        foreach (var error in errors)
            //        {
            //            _logger.LogWarning("Error de validación al editar Instructor: {Error}", error);

            //            if (error.Contains("fecha de contratación", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.HireDate), error.Trim());
            //            else if (error.Contains("18 años", StringComparison.OrdinalIgnoreCase) ||
            //                     error.Contains("nacimiento", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.DateBirth), error.Trim());
            //            else if (error.Contains("CI", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.Ci), error.Trim());
            //            else if (error.Contains("Especialización", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.Specialization), error.Trim());
            //            else if (error.Contains("Sueldo", StringComparison.OrdinalIgnoreCase) ||
            //                     error.Contains("salario", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.MonthlySalary), error.Trim());
            //            else if (error.Contains("correo", StringComparison.OrdinalIgnoreCase))
            //                ModelState.AddModelError(nameof(Instructor.Email), error.Trim());
            //            else
            //                ModelState.AddModelError(string.Empty, error.Trim());
            //        }

            //        return Page();
            //    }
            //}

            var updatedResult = await _userFacade.UpdateUserAsync(Instructor);

            if (updatedResult.IsFailure)
            {
                ModelState.AddModelError(string.Empty, updatedResult.Error);
                _logger.LogWarning("Error al actualizar instructor con ID {Id}", Instructor.Id);
                return Page();
            }

            TempData["SuccessMessage"] = $"Los datos de '{Instructor.Name} {Instructor.FirstLastname}' fueron actualizados correctamente.";
            _logger.LogInformation("Instructor actualizado correctamente: {Name} {Lastname}", Instructor.Name, Instructor.FirstLastname);
            return RedirectToPage("/Persons/Person");
        }
    }
}

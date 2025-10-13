using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Rules;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        public IActionResult OnPost()
        {
            // -------------------------
            // VALIDACIONES DE USUARIO
            // -------------------------
            if (!UserRules.NombreCompletoValido(User.Name))
                ModelState.AddModelError("User.Name", "Nombre inválido. Debe tener al menos 2 letras y solo letras y espacios.");

            if (!UserRules.NombreCompletoValido(User.FirstLastname))
                ModelState.AddModelError("User.FirstLastname", "Primer apellido inválido.");

            if (!UserRules.NombreCompletoValido(User.SecondLastname))
                ModelState.AddModelError("User.SecondLastname", "Segundo apellido inválido.");

            if (!UserRules.CiValido(User.Ci))
                ModelState.AddModelError("User.Ci", "CI inválido. Solo números y letras.");

            if (!UserRules.FechaNacimientoValida(User.DateBirth))
                ModelState.AddModelError("User.DateBirth", "La fecha de nacimiento no puede ser futura.");

            if (!UserRules.EsMayorDeEdad(User.DateBirth))
                ModelState.AddModelError("User.DateBirth", "El usuario debe ser mayor de edad para registrarse (mínimo 18 años).");

            if (!UserRules.RoleValido(User.Role))
                ModelState.AddModelError("User.Role", "Rol inválido.");

            // Si hay errores de validación, permanece en la página
            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // REDIRECCIÓN SEGÚN ROL
            // -------------------------
            if (User.Role == "Client")
            {
                return RedirectToPage("/Clients/Create", new
                {
                    name = User.Name,
                    firstLastname = User.FirstLastname,
                    secondLastname = User.SecondLastname,
                    ci = User.Ci,
                    dateBirth = User.DateBirth
                });
            }

            if (User.Role == "Instructor")
            {
                return RedirectToPage("/Instructors/Create", new
                {
                    name = User.Name,
                    firstLastname = User.FirstLastname,
                    secondLastname = User.SecondLastname,
                    ci = User.Ci,
                    dateBirth = User.DateBirth
                });
            }

            return Page();
        }
    }
}

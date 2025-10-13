using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Rules;
using GYMPT.Domain.Shared;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        // Diccionario para mapear roles a sus páginas de creación
        private readonly Dictionary<string, string> _rolePages = new()
        {
            { "Client", "/Clients/Create" },
            { "Instructor", "/Instructors/Create" }
        };

        public IActionResult OnPost()
        {
            // Validar el usuario usando Result
            ValidateUser();

            // Si hay errores, se queda en la página
            if (!ModelState.IsValid)
                return Page();

            // Redirigir según el rol
            if (_rolePages.TryGetValue(User.Role, out string page))
            {
                return RedirectToPage(page, new
                {
                    name = User.Name,
                    firstLastname = User.FirstLastname,
                    secondLastname = User.SecondLastname,
                    ci = User.Ci,
                    dateBirth = User.DateBirth
                });
            }

            // Si el rol no es válido o no soportado
            ModelState.AddModelError("User.Role", "Rol inválido o no soportado.");
            return Page();
        }

        // -------------------------
        // MÉTODO DE VALIDACIÓN
        // -------------------------
        private void ValidateUser()
        {
            AddModelErrorIfFail(UserRules.NombreCompletoValido(User.Name), "User.Name");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(User.FirstLastname), "User.FirstLastname");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(User.SecondLastname), "User.SecondLastname");
            AddModelErrorIfFail(UserRules.CiValido(User.Ci), "User.Ci");
            AddModelErrorIfFail(UserRules.FechaNacimientoValida(User.DateBirth), "User.DateBirth");
            AddModelErrorIfFail(UserRules.EsMayorDeEdad(User.DateBirth), "User.DateBirth");
            AddModelErrorIfFail(UserRules.RoleValido(User.Role), "User.Role");
        }

        // -------------------------
        // MÉTODO AUXILIAR PARA AGREGAR ERRORES
        // -------------------------
        private void AddModelErrorIfFail(Result result, string key)
        {
            if (result.IsFailure)
                ModelState.AddModelError(key, result.Error);
        }
    }
}

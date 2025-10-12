using GYMPT.Models;
using GYMPT.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        public IActionResult OnPost()
        {
            // Validaciones básicas de usuario
            if (!ValidacionesUsuario.EsNombreCompletoValido(User.Name))
                ModelState.AddModelError("User.Name", "Nombre inválido");

            if (!ValidacionesUsuario.EsNombreCompletoValido(User.FirstLastname))
                ModelState.AddModelError("User.FirstLastname", "Primer apellido inválido");

            if (!ValidacionesUsuario.EsNombreCompletoValido(User.SecondLastname))
                ModelState.AddModelError("User.SecondLastname", "Segundo apellido inválido");

            if (!ValidacionesUsuario.EsCiValido(User.Ci))
                ModelState.AddModelError("User.Ci", "CI inválido");

            if (!ValidacionesUsuario.EsFechaNacimientoValida(User.DateBirth))
                ModelState.AddModelError("User.DateBirth", "Fecha de nacimiento inválida");

            if (!ValidacionesUsuario.EsRoleValido(User.Role))
                ModelState.AddModelError("User.Role", "Rol inválido");

            if (!ModelState.IsValid)
                return Page();

            // Redirige a la página correspondiente pasando los datos como Query (no creamos DB todavía)
            if (User.Role == "Client")
                return RedirectToPage("/Clients/Create", new
                {
                    name = User.Name,
                    firstLastname = User.FirstLastname,
                    secondLastname = User.SecondLastname,
                    ci = User.Ci,
                    dateBirth = User.DateBirth
                });

            if (User.Role == "Instructor")
                return RedirectToPage("/Instructors/Create", new
                {
                    name = User.Name,
                    firstLastname = User.FirstLastname,
                    secondLastname = User.SecondLastname,
                    ci = User.Ci,
                    dateBirth = User.DateBirth
                });

            return Page();
        }
    }
}

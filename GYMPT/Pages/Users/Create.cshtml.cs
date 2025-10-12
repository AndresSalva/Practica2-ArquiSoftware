using GYMPT.Domain.Entities; // Asegúrate de tener el using para tu entidad User
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        // [BindProperty] conecta esta propiedad con el <select> en el formulario HTML.
        // Guardará el rol que el usuario elija ("Client" o "Instructor").
        [BindProperty]
        public User User { get; set; }

        // Este método se ejecuta cuando la página se carga. No necesita hacer nada.
        public void OnGet()
        {
        }

        // Este método se ejecuta cuando se presiona el botón "Continuar" (POST).
        public IActionResult OnPostAsync()
        {
            // Revisa si el modelo es válido (aunque en este caso simple, siempre lo será).
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Lógica de redirección:
            // Dependiendo del rol seleccionado, te envía a la página de creación detallada.
            if (User.Role == "Client")
            {
                // Asumo que tienes una página en /Pages/Clients/CreateClient.cshtml
                return RedirectToPage("/Clients/CreateClient");
            }

            if (User.Role == "Instructor")
            {
                // Asumo que tienes una página en /Pages/Instructors/CreateInstructor.cshtml
                return RedirectToPage("/Instructors/CreateInstructor");
            }

            // Si por alguna razón el rol no es ninguno de los dos, vuelve a la página.
            return Page();
        }
    }
}
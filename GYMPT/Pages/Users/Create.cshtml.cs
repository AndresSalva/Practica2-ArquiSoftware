using GYMPT.Domain.Entities; // Aseg�rate de tener el using para tu entidad User
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        // [BindProperty] conecta esta propiedad con el <select> en el formulario HTML.
        // Guardar� el rol que el usuario elija ("Client" o "Instructor").
        [BindProperty]
        public User User { get; set; }

        // Este m�todo se ejecuta cuando la p�gina se carga. No necesita hacer nada.
        public void OnGet()
        {
        }

        // Este m�todo se ejecuta cuando se presiona el bot�n "Continuar" (POST).
        public IActionResult OnPostAsync()
        {
            // Revisa si el modelo es v�lido (aunque en este caso simple, siempre lo ser�).
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // L�gica de redirecci�n:
            // Dependiendo del rol seleccionado, te env�a a la p�gina de creaci�n detallada.
            if (User.Role == "Client")
            {
                // Asumo que tienes una p�gina en /Pages/Clients/CreateClient.cshtml
                return RedirectToPage("/Clients/CreateClient");
            }

            if (User.Role == "Instructor")
            {
                // Asumo que tienes una p�gina en /Pages/Instructors/CreateInstructor.cshtml
                return RedirectToPage("/Instructors/CreateInstructor");
            }

            // Si por alguna raz�n el rol no es ninguno de los dos, vuelve a la p�gina.
            return Page();
        }
    }
}
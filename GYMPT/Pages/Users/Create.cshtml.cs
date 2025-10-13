using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (User.Role == "Client")
            {
                // ===== CORRECCIÓN CLAVE =====
                // Ahora redirige al nombre de archivo correcto: Create.cshtml
                return RedirectToPage("/Clients/Create");
            }

            if (User.Role == "Instructor")
            {
                // Asumo que la ruta para instructores también usa nombres genéricos
                return RedirectToPage("/Instructors/Create");
            }

            return Page();
        }
    }
}
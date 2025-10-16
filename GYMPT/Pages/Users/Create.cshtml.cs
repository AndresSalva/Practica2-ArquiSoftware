using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    [Authorize]
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
                // ===== CORRECCI�N CLAVE =====
                // Ahora redirige al nombre de archivo correcto: Create.cshtml
                return RedirectToPage("/Clients/Create");
            }

            if (User.Role == "Instructor")
            {
                // Asumo que la ruta para instructores tambi�n usa nombres gen�ricos
                return RedirectToPage("/Instructors/Create");
            }

            return Page();
        }
    }
}
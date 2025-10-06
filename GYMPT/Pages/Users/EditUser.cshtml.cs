using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private readonly UserRepository _userRepo;

        public EditUserModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public User UserData { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users/User");

            UserData = await _userRepo.GetByIdAsync(Id);

            if (UserData == null)
                return RedirectToPage("/Users/User");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UserData == null || Id == 0)
                return RedirectToPage("/Users/User");

            var updated = await _userRepo.UpdateAsync(UserData);

            if (updated == null)
            {
                TempData["Error"] = "No se pudo actualizar el usuario.";
                return Page();
            }

            // Redirigir según tipo de usuario
            if (updated.Role.Equals("Client", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Clients/EditClient", new { id = UserData.Id });
            }
            else if (updated.Role.Equals("Instructor", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Instructors/EditInstructor", new { id = UserData.Id });
            }
            // Si no es ninguno, volver a lista
            return RedirectToPage("/Users/User");
        }
    }
}

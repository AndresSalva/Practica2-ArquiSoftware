using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public EditUserModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public UserData User { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public bool IsClient => User?.Role == "Client";
        public bool IsInstructor => User?.Role == "Instructor";

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users");

            User = await _userRepo.GetByIdAsync(Id);

            if (User == null)
                return RedirectToPage("/Users");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null || Id == 0)
                return RedirectToPage("/Users");

            bool updated = await _userRepo.UpdateAsync(User);

            if (!updated)
            {
                TempData["Error"] = "No se pudo actualizar el usuario.";
                return Page();
            }

            // Redirigir según tipo de usuario
            if (User.Role == "Client")
            {
                return RedirectToPage("/Clients/EditClient", new { id = User.Id });
            }
            else if (User.Role == "Instructor")
            {
                return RedirectToPage("/Instructors/EditInstructor", new { id = User.Id });
            }

            // Si no es ninguno, volver a lista
            return RedirectToPage("/Users");
        }
    }
}

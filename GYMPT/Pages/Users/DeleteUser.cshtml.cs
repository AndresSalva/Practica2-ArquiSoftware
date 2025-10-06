using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class DeleteUserModel : PageModel
    {
        private readonly IUserRepository _userRepo;  // Cambiado a IUserRepository

        public DeleteUserModel(IUserRepository userRepo)  // Cambiado el parámetro
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public UserData User { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }  // Para recibir el id desde la URL

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users");

            User = await _userRepo.GetByIdAsync(Id);

            if (User == null)
                return RedirectToPage("/Users"); // Si no existe, volver a lista

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users");

            bool deleted = await _userRepo.DeleteAsync(Id);

            if (deleted)
            {
                TempData["Message"] = $"Usuario {User?.Name ?? Id.ToString()} eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el usuario.";
            }

            return RedirectToPage("/Users");
        }
    }
}

using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class DeleteUserModel : PageModel
    {
        private readonly UserRepository _userRepo;  // Cambiado a IUserRepository

        public DeleteUserModel(UserRepository userRepo)  // Cambiado el parámetro
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public User User { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }  // Para recibir el id desde la URL

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users/User");

            User = await _userRepo.GetByIdAsync(Id);

            if (User == null)
                return RedirectToPage("/Users/User"); // Si no existe, volver a lista

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users/User");

            bool deleted = await _userRepo.DeleteByIdAsync(Id);

            if (deleted)
            {
                TempData["Message"] = $"Usuario {User?.Name ?? Id.ToString()} eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo eliminar el usuario.";
            }

            return RedirectToPage("/Users/User");
        }
    }
}

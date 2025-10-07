using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class DeleteUserModel : PageModel
    {

        public DeleteUserModel() 
        {
        }
        private IRepository<User> CreateUserRepository()
        {
            var factory = new UserRepositoryCreator();
            return factory.CreateRepository();
        }

        [BindProperty]
        public User User { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }  // Para recibir el id desde la URL

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users/User");

            var repo = CreateUserRepository();

            User = await repo.GetByIdAsync(Id);

            if (User == null)
                return RedirectToPage("/Users/User"); 

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Id == 0)
                return RedirectToPage("/Users/User");

            var repo = CreateUserRepository();

            bool deleted = await repo.DeleteByIdAsync(Id);

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

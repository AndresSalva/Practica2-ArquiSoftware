using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<UserData> _repo;

        // Propiedad vinculada al formulario
        [BindProperty]
        public UserData User { get; set; } = new UserData();

        public CreateModel(IRepository<UserData> repo)
        {
            _repo = repo;
        }

        // GET -> solo carga la página vacía
        public void OnGet()
        {
        }

        // POST -> recibe datos del formulario y guarda
        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Role == "Client")
                return RedirectToPage("/Clients/Create");

            if (User.Role == "Instructor")
                return RedirectToPage("/Instructors/Create");

            return RedirectToPage("/Users/Users");
        }
    }
}

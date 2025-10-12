using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        public CreateModel()
        {
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost() 
        {
            if (User.Role == "Client")
            {
                return RedirectToPage("/Clients/Create");
            }

            if (User.Role == "Instructor")
            {
                return RedirectToPage("/Instructors/Create");
            }

            // Considera añadir un mensaje de error si el rol no es válido
            return RedirectToPage("/Users/User");
        }
    }
}
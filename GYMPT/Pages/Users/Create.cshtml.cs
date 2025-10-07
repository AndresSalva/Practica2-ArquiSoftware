using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

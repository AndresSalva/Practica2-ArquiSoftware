using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public Instructor Instructor { get; set; }

        public CreateModel()
        {
        }
        private IRepository<Instructor> CreateInstructorRepository()
        {
            var factory = new InstructorRepositoryCreator();
            return factory.CreateRepository();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var instructorRepo = CreateInstructorRepository();
            await instructorRepo.CreateAsync(Instructor);

            return RedirectToPage("/Users/User");
        }
    }
}

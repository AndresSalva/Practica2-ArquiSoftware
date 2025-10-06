using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        private readonly InstructorRepository _instructorRepo;

        [BindProperty]
        public Instructor Instructor { get; set; }

        public CreateModel(InstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Esto crea el User + Instructor en una sola transacción
            await _instructorRepo.CreateAsync(Instructor);

            return RedirectToPage("/Users");
        }
    }
}

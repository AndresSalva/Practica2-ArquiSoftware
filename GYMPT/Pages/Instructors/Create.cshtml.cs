using GYMPT.Data.Contracts;
using GYMPT.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        private readonly IInstructorRepository _instructorRepo;

        [BindProperty]
        public Instructor Instructor { get; set; } = new Instructor();

        public CreateModel(IInstructorRepository instructorRepo)
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

            return RedirectToPage("/Users/Users");
        }
    }
}

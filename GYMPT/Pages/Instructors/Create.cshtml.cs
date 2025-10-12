using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        [BindProperty]
        public Instructor Instructor { get; set; }

        public CreateModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _instructorService.CreateNewInstructor(Instructor);
            TempData["SuccessMessage"] = $"Instructor '{Instructor.Name}' creado exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
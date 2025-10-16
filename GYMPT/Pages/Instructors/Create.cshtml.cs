using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace GYMPT.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        [BindProperty]
        public Instructor Instructor { get; set; } = new();

        public CreateModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public void OnGet()
        {
            Instructor.Role = "Instructor";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Instructor.Role = "Instructor";
            await _instructorService.CreateNewInstructor(Instructor);

            TempData["SuccessMessage"] = $"El instructor '{Instructor.Name} {Instructor.FirstLastname}' ha sido creado exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
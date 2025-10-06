using GYMPT.Data.Contracts;
using GYMPT.Domain;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {
        private readonly IInstructorRepository _instructorRepo;

        public EditInstructorModel(IInstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0) return RedirectToPage("/Users");

            Instructor = await _instructorRepo.GetByIdAsync(Id);
            if (Instructor == null) return RedirectToPage("/Users");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Instructor == null || Id == 0) return RedirectToPage("/Users");

            await _instructorRepo.UpdateAsync(Instructor);

            TempData["Message"] = $"Instructor {Instructor.Name} actualizado correctamente.";
            return RedirectToPage("/Users");
        }
    }
}

using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {
        private readonly InstructorRepository _instructorRepo;

        public EditInstructorModel(InstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Instructor = await _instructorRepo.GetByIdAsync(Id);
            if (Instructor == null) return RedirectToPage("/Users/User");
            Id = Instructor.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Instructor == null || Id == 0) return RedirectToPage("/Users/User");
            Instructor.Id = Id;
            var updated = await _instructorRepo.UpdateAsync(Instructor);

            TempData["Message"] = $"Instructor {Instructor.Name} actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}

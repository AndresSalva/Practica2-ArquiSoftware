using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        [BindProperty]
        public Instructor Instructor { get; set; }

        public EditModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Instructor = await _instructorService.GetInstructorById(id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Users/User");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _instructorService.UpdateInstructorData(Instructor);

            TempData["SuccessMessage"] = "Los datos del instructor han sido actualizados exitosamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
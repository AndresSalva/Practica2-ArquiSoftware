using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.SpecificUserDetail
{
    public class InstructorDetailsModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        public Instructor Instructor { get; set; }

        public InstructorDetailsModel(IInstructorService instructorService)
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
    }
}
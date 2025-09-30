using GYMPT.Data.Contracts;
using GYMPT.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class InstructorDetailsModel : PageModel
    {
        private readonly IInstructorRepository _instructorRepo;

        public Instructor Instructor { get; set; }

        public InstructorDetailsModel(IInstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            // Llamamos al repositorio específico para instructores.
            Instructor = await _instructorRepo.GetByIdAsync(id);

            if (Instructor == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
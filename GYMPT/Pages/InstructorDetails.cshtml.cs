using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class InstructorDetailsModel : PageModel
    {
        private readonly InstructorRepository _instructorRepo;

        public Instructor Instructor { get; set; }

        public InstructorDetailsModel(InstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
 
            Instructor = await _instructorRepo.GetByIdAsync(id);

            if (Instructor == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
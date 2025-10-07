using GYMPT.Factories; 
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;


namespace GYMPT.Pages.SpecificUserDetail
{
    public class InstructorDetailsModel : PageModel
    {
        public Instructor Instructor { get; set; }


        public InstructorDetailsModel()
        {
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var factory = new InstructorRepositoryCreator();
            var instructorRepo = factory.CreateRepository();

            Instructor = await instructorRepo.GetByIdAsync(id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "Instructor no encontrado.";
                return RedirectToPage("/Users/User");
            }

            return Page();
        }
    }
}
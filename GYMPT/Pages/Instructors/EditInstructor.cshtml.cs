using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {

        public EditInstructorModel()
        {
        }
        private IRepository<Instructor> CreateInstructorRepository()
        {
            var factory = new InstructorRepositoryCreator();
            return factory.CreateRepository();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var instructorRepo = CreateInstructorRepository();
            Instructor = await instructorRepo.GetByIdAsync(Id);
            if (Instructor == null) return RedirectToPage("/Users/User");
            Id = Instructor.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Instructor == null || Id == 0) return RedirectToPage("/Users/User");
            Instructor.Id = Id;
            var instructorRepo = CreateInstructorRepository();
            var updated = await instructorRepo.UpdateAsync(Instructor);

            TempData["Message"] = $"Instructor {Instructor.Name} actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}

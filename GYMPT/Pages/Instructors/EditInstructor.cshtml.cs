using GYMPT.Data.Contracts;
using GYMPT.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            Console.WriteLine($"[DEBUG] Valor recibido de Id: {Id}");

            if (Id == 0)
            {
                Console.WriteLine("[DEBUG] Id = 0, redirigiendo a /Users");
                return RedirectToPage("/Users");
            }

            Instructor = await _instructorRepo.GetByIdAsync(Id);
            if (Instructor == null)
            {
                Console.WriteLine($"[DEBUG] No se encontró instructor con id {Id}");
                return RedirectToPage("/Users");
            }

            Console.WriteLine($"[DEBUG] Instructor encontrado: {Instructor.Name}");
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

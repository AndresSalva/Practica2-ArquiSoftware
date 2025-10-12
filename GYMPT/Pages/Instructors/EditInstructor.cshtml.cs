using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public EditInstructorModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Instructor = await _instructorService.GetInstructorById(Id);

            if (Instructor == null)
            {
                TempData["ErrorMessage"] = "El instructor que intentas editar no fue encontrado.";
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

            Instructor.Id = Id;
            await _instructorService.UpdateInstructorData(Instructor);
            TempData["SuccessMessage"] = $"Instructor '{Instructor.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
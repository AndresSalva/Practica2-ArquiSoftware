using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEditModel(DisciplineRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Discipline = await _repo.GetByIdAsync(id);

            if (Discipline == null)
            {
                return RedirectToPage("Disciplines/Discipline");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.UpdateAsync(Discipline);

            TempData["Message"] = $"Disciplina '{Discipline.Name}' actualizada correctamente ✅";

            return RedirectToPage("Disciplines/Discipline");
        }
    }
}

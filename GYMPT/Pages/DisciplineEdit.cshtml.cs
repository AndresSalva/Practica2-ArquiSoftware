using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages
{
    public class DisciplineEdit : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEdit(DisciplineRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Discipline = await _repo.GetByIdAsync(id);

            if (Discipline == null)
            {
                return RedirectToPage("./Disciplines");
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

            return RedirectToPage("./Disciplines");
        }
    }
}

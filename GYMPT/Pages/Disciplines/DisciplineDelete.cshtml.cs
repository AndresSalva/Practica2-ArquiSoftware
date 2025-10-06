using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineDeleteModel : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineDeleteModel(DisciplineRepository repo)
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
            if (Discipline == null || Discipline.Id == 0)
            {
                return Page();
            }

            await _repo.DeleteByIdAsync(Discipline.Id);

            return RedirectToPage("Disciplines/Discipline");
        }
    }
}

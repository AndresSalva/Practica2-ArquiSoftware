using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class DisciplineEdit : PageModel
    {
        private readonly IRepository<Discipline> _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEdit(IRepository<Discipline> repo)
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

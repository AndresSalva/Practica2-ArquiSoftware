using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages
{
    public class DisciplineDelete : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineDelete(DisciplineRepository repo)
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
            if (Discipline == null || Discipline.Id == 0)
            {
                return Page();
            }

            await _repo.DeleteByIdAsync(Discipline.Id);

            return RedirectToPage("./Disciplines");
        }
    }
}

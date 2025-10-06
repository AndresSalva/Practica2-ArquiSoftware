using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineModel : PageModel
    {
        private readonly DisciplineRepository _repo;

        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplineModel(DisciplineRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            var allDisciplines = await _repo.GetAllAsync();
            DisciplineList = allDisciplines.Where(d => d.IsActive == true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _repo.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}

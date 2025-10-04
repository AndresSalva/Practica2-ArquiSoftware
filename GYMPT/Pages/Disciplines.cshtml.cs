using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class DisciplinesModel : PageModel
    {
        private readonly IRepository<Discipline> _repo;

        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplinesModel(IRepository<Discipline> repo)
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

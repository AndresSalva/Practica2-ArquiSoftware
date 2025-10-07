using GYMPT.Data.Contracts;
using GYMPT.Factories; 
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineModel : PageModel
    {
        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplineModel() { }

        private IRepository<Discipline> CreateDisciplineRepository()
        {
            var factory = new DisciplineRepositoryCreator();
            return factory.CreateRepository();
        }

        public async Task OnGetAsync()
        {
            var repo = CreateDisciplineRepository();
            var allDisciplines = await repo.GetAllAsync();
            DisciplineList = allDisciplines.Where(d => d.IsActive == true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var repo = CreateDisciplineRepository();
            await repo.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}
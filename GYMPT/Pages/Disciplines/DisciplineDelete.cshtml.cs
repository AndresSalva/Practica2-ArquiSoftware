using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineDeleteModel : PageModel
    {

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineDeleteModel()
        {
        }
        private IRepository<Discipline> CreateDisciplineRepository()
        {
            var factory = new DisciplineRepositoryCreator();
            return factory.CreateRepository();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var repo = CreateDisciplineRepository();
            Discipline = await repo.GetByIdAsync(id);

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
            var repo = CreateDisciplineRepository();

            await repo.DeleteByIdAsync(Discipline.Id);

            return RedirectToPage("Disciplines/Discipline");
        }
    }
}

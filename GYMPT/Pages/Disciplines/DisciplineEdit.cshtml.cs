using GYMPT.Data.Contracts;
using GYMPT.Factories; 
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEditModel() { }

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
                return RedirectToPage("/Disciplines/Discipline");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var repo = CreateDisciplineRepository();

            await repo.UpdateAsync(Discipline);

            TempData["Message"] = $"Disciplina '{Discipline.Name}' actualizada correctamente";

            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
using GYMPT.Data.Contracts;
using GYMPT.Factories; // <-- PASO 2
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineCreateModel : PageModel
    {
        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineCreateModel() { }
        private IRepository<Discipline> CreateDisciplineRepository()
        {
            var factory = new DisciplineRepositoryCreator();
            return factory.CreateRepository();
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var repo = CreateDisciplineRepository();

            await repo.CreateAsync(Discipline);

            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
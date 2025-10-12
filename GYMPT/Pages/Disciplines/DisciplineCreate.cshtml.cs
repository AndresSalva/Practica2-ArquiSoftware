using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineCreateModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineCreateModel(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _disciplineService.CreateNewDiscipline(Discipline);
            TempData["SuccessMessage"] = $"Disciplina '{Discipline.Name}' creada exitosamente.";
            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
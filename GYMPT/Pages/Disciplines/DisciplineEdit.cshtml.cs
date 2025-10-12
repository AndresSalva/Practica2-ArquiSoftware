using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEditModel(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Discipline = await _disciplineService.GetDisciplineById(id);

            if (Discipline == null)
            {
                TempData["ErrorMessage"] = "La disciplina que intentas editar no fue encontrada.";
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

            await _disciplineService.UpdateDisciplineData(Discipline);
            TempData["SuccessMessage"] = $"Disciplina '{Discipline.Name}' actualizada correctamente.";
            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineDeleteModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineDeleteModel(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _disciplineService.GetDisciplineById(id);
            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = "No se pudo cargar la disciplina.";
                return RedirectToPage("/Disciplines/Discipline");
            }

            Discipline = result.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Discipline?.Id == 0)
            {
                return Page();
            }

            await _disciplineService.DeleteDiscipline(Discipline.Id);

            TempData["SuccessMessage"] = "Disciplina eliminada correctamente.";
            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
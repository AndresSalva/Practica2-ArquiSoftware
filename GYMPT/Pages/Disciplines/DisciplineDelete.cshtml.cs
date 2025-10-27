using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            Discipline = await _disciplineService.GetDisciplineById(id);

            if (Discipline == null)
            {
                TempData["ErrorMessage"] = "La disciplina que intentas eliminar no fue encontrada.";
                return RedirectToPage("/Disciplines/Discipline");
            }

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
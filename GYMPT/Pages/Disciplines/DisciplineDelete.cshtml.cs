// Los usings que tienes ya están correctos para esta página.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks; // <-- Añadido para consistencia

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
            // --- CAMBIO: Usar el nombre de método correcto del nuevo contrato ---
            Discipline = await _disciplineService.GetByIdAsync(id); // El método correcto es GetByIdAsync

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

            // --- CAMBIO: Usar el nombre de método correcto del nuevo contrato ---
            await _disciplineService.DeleteByIdAsync(Discipline.Id); // El método correcto es DeleteByIdAsync

            TempData["SuccessMessage"] = "Disciplina eliminada correctamente.";
            return RedirectToPage("/Disciplines/Discipline");
        }
    }
}
// Los usings que tienes ya están correctos para esta página.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks; // <-- Añadido para consistencia

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineCreateModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly ISelectDataService _selectDataService;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();
        public SelectList InstructorOptions { get; set; } = default!;

        public DisciplineCreateModel(IDisciplineService disciplineService, ISelectDataService selectDataService)
        {
            _disciplineService = disciplineService;
            _selectDataService = selectDataService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            InstructorOptions = await _selectDataService.GetInstructorOptionsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                InstructorOptions = await _selectDataService.GetInstructorOptionsAsync();
                return Page();
            }

            // --- CAMBIO: Usar el nombre de método correcto del nuevo contrato ---
            await _disciplineService.CreateAsync(Discipline); // El método correcto es CreateAsync

            TempData["SuccessMessage"] = $"La disciplina '{Discipline.Name}' ha sido creada exitosamente.";
            return RedirectToPage("./Discipline");
        }
    }
}
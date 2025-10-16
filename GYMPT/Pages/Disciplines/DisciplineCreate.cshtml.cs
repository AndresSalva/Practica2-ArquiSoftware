using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineCreateModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly ISelectDataService _selectDataService; // <-- Se usa el servicio especializado

        [BindProperty]
        public Discipline Discipline { get; set; } = new();
        public SelectList InstructorOptions { get; set; }

        public DisciplineCreateModel(IDisciplineService disciplineService, ISelectDataService selectDataService)
        {
            _disciplineService = disciplineService;
            _selectDataService = selectDataService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // La página simplemente pide los datos para el dropdown, ya listos para usar.
            InstructorOptions = await _selectDataService.GetInstructorOptionsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Si hay un error, debemos volver a cargar el dropdown antes de mostrar la página de nuevo.
                InstructorOptions = await _selectDataService.GetInstructorOptionsAsync();
                return Page();
            }

            await _disciplineService.CreateNewDiscipline(Discipline);
            TempData["SuccessMessage"] = $"La disciplina '{Discipline.Name}' ha sido creada exitosamente.";
            return RedirectToPage("./Discipline");
        }
    }
}
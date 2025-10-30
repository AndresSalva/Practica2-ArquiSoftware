using GYMPT.Application.Interfaces;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
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
            await PopulateInstructorOptionsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateInstructorOptionsAsync();
                return Page();
            }

            var result = await _disciplineService.CreateNewDiscipline(Discipline);

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                await PopulateInstructorOptionsAsync();
                return Page();

            }

            TempData["SuccessMessage"] = $"Disciplina '{result.Value.Name}' creada exitosamente.";
            return RedirectToPage("./Discipline");
        }
        
        private async Task PopulateInstructorOptionsAsync()
        {
            InstructorOptions = await _selectDataService.GetInstructorOptionsAsync();
        }
    }
}
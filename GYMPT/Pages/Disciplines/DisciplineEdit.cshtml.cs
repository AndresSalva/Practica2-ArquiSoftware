using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly SelectDataFacade _facade;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public Discipline Discipline { get; set; } = default!;
        public SelectList InstructorOptions { get; set; } = default!;

        public DisciplineEditModel(
            IDisciplineService disciplineService,
            SelectDataFacade facade,
            UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _facade = facade;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenId = _urlTokenSingleton.Unprotect(token);
            if (tokenId == null)
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("./Discipline");
            }
            int id = int.Parse(tokenId);
            var result = await _disciplineService.GetDisciplineById(id);

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToPage("./Discipline");
            }

            Discipline = result.Value;

            await PopulateInstructorsDropDownList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateInstructorsDropDownList();
                return Page();
            }

            var result = await _disciplineService.UpdateDiscipline(Discipline);

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                await PopulateInstructorsDropDownList();
                return Page();
            }

            TempData["SuccessMessage"] = "Los datos de la disciplina han sido actualizados.";
            return RedirectToPage("./Discipline");
        }

        private async Task PopulateInstructorsDropDownList()
        {
            var instructors = await _facade.GetInstructorOptionsAsync();
            InstructorOptions = new SelectList(
                instructors,
                "Id",
                "FullName",
                Discipline?.IdInstructor
            );
        }
    }
}

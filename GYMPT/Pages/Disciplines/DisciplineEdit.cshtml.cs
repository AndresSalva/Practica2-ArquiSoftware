using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
        public Discipline Discipline { get; set; }
        public SelectList InstructorOptions { get; set; }

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
            var tokenId = _urlTokenSingleton.GetTokenData(token);
            if (tokenId == null)
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("./Discipline");
            }

            int id = int.Parse(tokenId);
            Discipline = await _disciplineService.GetDisciplineById(id);
            if (Discipline == null)
            {
                TempData["ErrorMessage"] = "Disciplina no encontrada.";
                return RedirectToPage("./Discipline");
            }

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

            var success = await _disciplineService.UpdateDisciplineData(Discipline);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] =
                success ? "Los datos de la disciplina han sido actualizados."
                        : "No se pudo actualizar la disciplina.";

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

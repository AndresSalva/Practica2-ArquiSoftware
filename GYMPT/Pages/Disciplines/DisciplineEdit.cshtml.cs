using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Security.Policy;
using Mono.TextTemplating;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineEditModel(IDisciplineService disciplineService, UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            int id = int.Parse(_urlTokenSingleton.GetTokenData(token));
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
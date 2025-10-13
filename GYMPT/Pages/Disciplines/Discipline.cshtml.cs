using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly UrlTokenSingleton _urlTokenSingleton;
        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplineModel(IDisciplineService disciplineService, UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            var allDisciplines = await _disciplineService.GetAllDisciplines();
            DisciplineList = allDisciplines.Where(d => d.IsActive);
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            // Generate a route token using the UrlTokenSingleton and redirect to the edit page
            string token = _urlTokenSingleton.GenerateToken(id.ToString());
            return RedirectToPage("/Disciplines/DisciplineEdit", new { token });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _disciplineService.DeleteDiscipline(id);
            TempData["SuccessMessage"] = "Disciplina eliminada correctamente.";
            return RedirectToPage();
        }
    }
}
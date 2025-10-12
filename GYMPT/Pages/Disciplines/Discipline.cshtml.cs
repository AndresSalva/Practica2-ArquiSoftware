using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplineModel(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        public async Task OnGetAsync()
        {
            var allDisciplines = await _disciplineService.GetAllDisciplines();
            DisciplineList = allDisciplines.Where(d => d.IsActive);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _disciplineService.DeleteDiscipline(id);
            TempData["SuccessMessage"] = "Disciplina eliminada correctamente.";
            return RedirectToPage();
        }
    }
}
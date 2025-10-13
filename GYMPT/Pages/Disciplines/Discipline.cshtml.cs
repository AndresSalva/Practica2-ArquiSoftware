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
        private readonly IUserService _userService;
        public IEnumerable<Discipline> DisciplineList { get; set; } = new List<Discipline>();
        public Dictionary<long, string> InstructorNames { get; set; } = new Dictionary<long, string>();

        public DisciplineModel(IDisciplineService disciplineService, IUserService userService)
        {
            _disciplineService = disciplineService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            DisciplineList = await _disciplineService.GetAllDisciplines();
            var users = await _userService.GetAllUsers();
            InstructorNames = users.ToDictionary(u => (long)u.Id, u => $"{u.Name} {u.FirstLastname}");
        }

        // El método recibe un 'int' desde la vista
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _disciplineService.DeleteDiscipline(id);
            if (success)
            {
                TempData["SuccessMessage"] = "La disciplina ha sido eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la disciplina.";
            }
            return RedirectToPage();
        }
    }
}
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IPersonService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;
        public IEnumerable<Discipline> DisciplineList { get; set; } = new List<Discipline>();
        public Dictionary<long, string> InstructorNames { get; set; } = new Dictionary<long, string>();

        public DisciplineModel(IDisciplineService disciplineService, IPersonService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            DisciplineList = await _disciplineService.GetAllDisciplines();
            var users = await _userService.GetAllUsers();
            InstructorNames = users.ToDictionary(u => (long)u.Id, u => $"{u.Name} {u.FirstLastname}");
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            // Generate a route token using the UrlTokenSingleton and redirect to the edit page
            string token = _urlTokenSingleton.GenerateToken(id.ToString());
            return RedirectToPage("/Disciplines/DisciplineEdit", new { token });
        }

        // El m�todo recibe un 'int' desde la vista
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
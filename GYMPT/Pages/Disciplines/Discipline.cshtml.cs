using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using GYMPT.Infrastructure.Services;
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceClient.Application.Interfaces;
using ServiceClient.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public IEnumerable<Discipline> DisciplineList { get; set; } = new List<Discipline>();
        public Dictionary<long, string> InstructorNames { get; set; } = new Dictionary<long, string>();

        public DisciplineModel(IDisciplineService disciplineService, IUserService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            // --- CAMBIO 2: Estandarizar las llamadas a los métodos ---
            DisciplineList = await _disciplineService.GetAllAsync(); // Asumiendo que el método estándar es GetAllAsync
            var users = await _userService.GetAllAsync();           // El método correcto es GetAllAsync
            InstructorNames = users.ToDictionary(u => (long)u.Id, u => $"{u.Name} {u.FirstLastname}");
        }

        public IActionResult OnPostEditAsync(int id) // No necesita 'async' porque no hay 'await'
        {
            string token = _urlTokenSingleton.GenerateToken(id.ToString());
            return RedirectToPage("/Disciplines/DisciplineEdit", new { token });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _disciplineService.DeleteDiscipline(id);

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la disciplina.";
                ModelState.AddModelError(string.Empty, result.Error);
            }
            else
            {
                TempData["SuccessMessage"] = "Disciplina eliminada exitosamente.";

            }
            return RedirectToPage();
        }
    }
}
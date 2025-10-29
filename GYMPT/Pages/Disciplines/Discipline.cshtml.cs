// --- CAMBIO 1: Corregir las directivas 'using' ---
// AÚN necesitamos los 'usings' antiguos para los servicios que no se han movido.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// Se necesita el 'using' del nuevo módulo para IUserService.
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
        private readonly ParameterProtector _urlTokenSingleton;
        public IEnumerable<Discipline> DisciplineList { get; set; } = new List<Discipline>();
        public Dictionary<long, string> InstructorNames { get; set; } = new Dictionary<long, string>();

        public DisciplineModel(IDisciplineService disciplineService, IUserService userService, ParameterProtector urlTokenSingleton)
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
            // Generate a route token using the UrlTokenSingleton and redirect to the edit page
            string token = _urlTokenSingleton.Protect(id.ToString());
            return RedirectToPage("/Disciplines/DisciplineEdit", new { token });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var success = await _disciplineService.DeleteByIdAsync(id); // Asumiendo que el método estándar es DeleteByIdAsync
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
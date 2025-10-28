// --- CAMBIO 1: Corregir las directivas 'using' ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService.
using ServiceClient.Application.Interfaces;

// AÚN necesitamos los 'usings' antiguos para los servicios que no se han movido.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public Discipline Discipline { get; set; } = default!;
        public SelectList InstructorOptions { get; set; } = default!;

        public DisciplineEditModel(IDisciplineService disciplineService, IUserService userService, UrlTokenSingleton urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenId = _urlTokenSingleton.GetTokenData(token);
            if (tokenId == null || !int.TryParse(tokenId, out var id))
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("./Discipline");
            }

            // --- CAMBIO 2: Estandarizar las llamadas a los métodos ---
            Discipline = await _disciplineService.GetByIdAsync(id); // El método correcto es GetByIdAsync
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

            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var updatedDiscipline = await _disciplineService.UpdateAsync(Discipline); // El método correcto es UpdateAsync

            if (updatedDiscipline != null)
            {
                TempData["SuccessMessage"] = "Los datos de la disciplina han sido actualizados.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo actualizar la disciplina.";
            }

            return RedirectToPage("./Discipline");
        }

        private async Task PopulateInstructorsDropDownList()
        {
            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var users = await _userService.GetAllAsync(); // El método correcto es GetAllAsync
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new { Id = (long)u.Id, FullName = $"{u.Name} {u.FirstLastname}" });

            InstructorOptions = new SelectList(instructors, "Id", "FullName", Discipline?.IdInstructor);
        }
    }
}
using GYMPT.Infrastructure.Services;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceClient.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;
        private readonly ParameterProtector _urlTokenSingleton;

        [BindProperty]
        public Discipline Discipline { get; set; } = default!;
        public SelectList InstructorOptions { get; set; } = default!;

        public DisciplineEditModel(IDisciplineService disciplineService, IUserService userService, ParameterProtector urlTokenSingleton)
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
            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var users = await _userService.GetAllAsync(); // El método correcto es GetAllAsync
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new { Id = u.Id, FullName = $"{u.Name} {u.FirstLastname}" });

            InstructorOptions = new SelectList(instructors, "Id", "FullName", Discipline?.IdInstructor);
        }
    }
}
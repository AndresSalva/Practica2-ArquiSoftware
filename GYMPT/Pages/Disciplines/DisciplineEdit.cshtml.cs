using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceCommon.Infrastructure.Services;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using ServiceUser.Application.Interfaces;

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;
        private readonly ParameterProtector _urlTokenSingleton;

        [BindProperty]
        public Discipline Discipline { get; set; }
        public SelectList InstructorOptions { get; set; }

        public DisciplineEditModel(IDisciplineService disciplineService, IUserService userService, ParameterProtector urlTokenSingleton)
        {
            _disciplineService = disciplineService;
            _userService = userService;
            _urlTokenSingleton = urlTokenSingleton;
        }
        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenId = _urlTokenSingleton.Unprotect(token);
            if (tokenId == null)
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
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new { Id = u.Id, FullName = $"{u.Name} {u.FirstLastname}" });

            InstructorOptions = new SelectList(instructors, "Id", "FullName", Discipline?.IdUser);
        }
    }
}
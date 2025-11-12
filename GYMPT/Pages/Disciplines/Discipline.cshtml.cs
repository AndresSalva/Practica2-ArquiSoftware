using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Application.Interfaces;
using ServiceCommon.Infrastructure.Services;

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
            DisciplineList = await _disciplineService.GetAllDisciplines();
            var users = await _userService.GetAllUsers();
            InstructorNames = users.ToDictionary(u => (long)u.Id, u => $"{u.Name} {u.FirstLastname}");
        }

        public IActionResult OnPostEditAsync(int id)
        {
            string token = _urlTokenSingleton.Protect(id.ToString());
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
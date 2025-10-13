using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineCreateModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();
        public SelectList InstructorOptions { get; set; }

        public DisciplineCreateModel(IDisciplineService disciplineService, IUserService userService)
        {
            _disciplineService = disciplineService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            await PopulateInstructorsDropDownList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateInstructorsDropDownList();
                return Page();
            }

            await _disciplineService.CreateNewDiscipline(Discipline);
            TempData["SuccessMessage"] = $"La disciplina '{Discipline.Name}' ha sido creada exitosamente.";
            return RedirectToPage("./Discipline");
        }

        private async Task PopulateInstructorsDropDownList()
        {
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new { Id = (long)u.Id, FullName = $"{u.Name} {u.FirstLastname}" });
            InstructorOptions = new SelectList(instructors, "Id", "FullName");
        }
    }
}
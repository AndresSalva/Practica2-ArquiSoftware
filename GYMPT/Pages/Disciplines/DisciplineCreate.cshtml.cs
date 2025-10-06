using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineCreateModel : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineCreateModel(DisciplineRepository repo)
        {
            _repo = repo;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.CreateAsync(Discipline);

            return RedirectToPage("Disciplines/Discipline");
        }
    }
}

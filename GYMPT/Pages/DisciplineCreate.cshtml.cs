using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using GYMPT.Data.Repositories;

namespace GYMPT.Pages
{
    public class DisciplineCreate : PageModel
    {
        private readonly DisciplineRepository _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineCreate(DisciplineRepository repo)
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

            return RedirectToPage("./Disciplines");
        }
    }
}

using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class DisciplineCreate : PageModel
    {
        private readonly IRepository<Discipline> _repo;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public DisciplineCreate(IRepository<Discipline> repo)
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

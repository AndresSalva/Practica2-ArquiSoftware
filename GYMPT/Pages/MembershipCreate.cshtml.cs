using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class MembershipCreateModel : PageModel
    {
        private readonly IRepository<Membership> _repo;

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipCreateModel(IRepository<Membership> repo)
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

            await _repo.CreateAsync(Membership);

            return RedirectToPage("./Memberships");
        }
    }
}
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
    public class MembershipCreateModel : PageModel
    {
        private readonly MembershipRepository _repo;

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipCreateModel(MembershipRepository repo)
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
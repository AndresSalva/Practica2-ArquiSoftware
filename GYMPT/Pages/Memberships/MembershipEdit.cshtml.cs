using GYMPT.Models;
using GYMPT.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace GYMPT.Pages.Memberships
{
    public class MembershipEditModel : PageModel
    {
        private readonly MembershipRepository _repo;

        private readonly IConfiguration _configuration;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel(MembershipRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(short id)
        {
            Membership = await _repo.GetByIdAsync(id);

            if (Membership == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.UpdateAsync(Membership);

            return RedirectToPage("/Memberships/Membership");
        }
    }
}